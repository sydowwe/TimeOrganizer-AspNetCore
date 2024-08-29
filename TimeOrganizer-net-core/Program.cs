using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core;
using TimeOrganizer_net_core.config;
using TimeOrganizer_net_core.exception;
using TimeOrganizer_net_core.helper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
  );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyInjection();

builder.Services.AddIdentityServices();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        corsBuilder =>
        {
            corsBuilder.WithOrigins("http://localhost:3000","http://localhost:5104", Environment.GetEnvironmentVariable("CORS_ENABLED_URL") ?? throw new EnvironmentVariableMissingException("CORS_ENABLED_URL"))
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.MaxAge = TimeSpan.FromHours(3);
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// app.UseHttpsRedirection();
app.UsePathBase(new PathString("/api"));
app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseSession(); 

app.MapControllers();

app.Run();