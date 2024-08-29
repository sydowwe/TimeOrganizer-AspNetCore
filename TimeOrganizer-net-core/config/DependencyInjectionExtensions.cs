using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO.mapper;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.config;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        //Repository
        services.AddScoped<IActivityHistoryRepository, ActivityHistoryRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IAlarmRepository, AlarmRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPlannerTaskRepository, PlannerTaskRepository>();
        services.AddScoped<IPlannerTaskRepository, PlannerTaskRepository>();
        services.AddScoped<IRoutineToDoListRepository, RoutineToDoListRepository>();
        services.AddScoped<IRoutineTimePeriodRepository, RoutineTimePeriodRepository>();
        services.AddScoped<IToDoListRepository, ToDoListRepository>();
        services.AddScoped<ITaskUrgencyRepository, TaskUrgencyRepository>();
        services.AddScoped<IWebExtensionDataRepository, WebExtensionDataRepository>();
        
        //User Service
        services.AddScoped<ILoggedUserService, LoggedUserService>();
        services.AddScoped<IUserService, UserService>();
        services.AddHttpClient<IGoogleRecaptchaService, GoogleRecaptchaService>();
        services.AddSingleton<IGoogleRecaptchaService, GoogleRecaptchaService>();
        // Configure mail settings
        // services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
        services.AddTransient<IEmailService, EmailService>();

        //Service
        services.AddScoped<IAlarmService, AlarmService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IActivityHistoryService, ActivityHistoryService>();
        services.AddScoped<IPlannerTaskService, PlannerTaskService>();
        services.AddScoped<IPlannerTaskService, PlannerTaskService>();
        services.AddScoped<IRoutineToDoListService, RoutineToDoListService>();
        services.AddScoped<IRoutineTimePeriodService, RoutineTimePeriodService>();
        services.AddScoped<IToDoListService, ToDoListService>();
        services.AddScoped<ITaskUrgencyService, TaskUrgencyService>();
        services.AddScoped<IWebExtensionDataService, WebExtensionDataService>();
        
        //MAPPER profiles
        services.AddAutoMapper(typeof(ActivityProfile).Assembly);
        return services;
    }
}