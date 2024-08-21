using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOrganizer_net_core;
using TimeOrganizer_net_core.model.DTO.mapper;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Configure Entity Framework Core to use PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IAlarmRepository, AlarmRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddScoped<IPlannerTaskRepository, PlannerTaskRepository>();
builder.Services.AddScoped<IPlannerTaskRepository, PlannerTaskRepository>();
builder.Services.AddScoped<IRoutineTimePeriodRepository, RoutineTimePeriodRepository>();
builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<ITaskUrgencyRepository, TaskUrgencyRepository>();
builder.Services.AddScoped<IWebExtensionDataRepository, WebExtensionDataRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAlarmService, AlarmService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IPlannerTaskService, PlannerTaskService>();
builder.Services.AddScoped<IPlannerTaskService, PlannerTaskService>();
builder.Services.AddScoped<IRoutineTimePeriodService, RoutineTimePeriodService>();
builder.Services.AddScoped<IToDoListService, ToDoListService>();
builder.Services.AddScoped<ITaskUrgencyService, TaskUrgencyService>();
builder.Services.AddScoped<IWebExtensionDataService, WebExtensionDataService>();

// typeof(IRepository<>).Assembly.GetTypes()
//     .Where(type => typeof(IRepository<>).IsAssignableFrom(type) && !type.IsAbstract)
//     .ToList().ForEach(type => builder.Services.AddScoped(type));

builder.Services.AddAutoMapper(typeof(ActivityProfile).Assembly);

// Configure mail settings
// builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Configure OAuth2 authentication
// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//     })
//     .AddCookie()
//     .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
//     {
//         options.ClientId = builder.Configuration["OAuth2:Google:ClientId"];
//         options.ClientSecret = builder.Configuration["OAuth2:Google:ClientSecret"];
//         options.CallbackPath = new PathString(builder.Configuration["OAuth2:Google:RedirectUri"]);
//     });

// JWT
// builder.Services.AddSingleton<IJwtService, JwtService>();
// builder.Services.addJwtAuthentication(builder.Configuration);
builder.Services.AddIdentityServices();


builder.Services.AddDistributedMemoryCache(); // You can replace this with Redis for distributed caching
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.MaxAge = TimeSpan.FromHours(3);
});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.MaxAge = TimeSpan.FromHours(3);

        options.SlidingExpiration = true;
        options.LoginPath = "/api/user/auth/login";
        options.LogoutPath = "/api/user/auth/logout";
        options.Events.OnValidatePrincipal = async context =>
        {
            // Custom validation logic
            await Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHttpsRedirection();


app.Run();