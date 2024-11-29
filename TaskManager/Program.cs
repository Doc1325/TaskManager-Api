using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManager.Controllers;
using TaskManager.Dtos;
using TaskManager.Mappers;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Services;
using TaskManager.Validators;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("all",
 builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers();

//Servicios

builder.Services.AddKeyedScoped<ITaskservice, TaskService>("TaskService");
builder.Services.AddKeyedScoped <ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int>,StatusService>("StatusService");
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddHttpContextAccessor();

//Repositorios y DBContext
builder.Services.AddKeyedScoped<IRepository<TaskItems>, TaskRepository>("Tasks");
builder.Services.AddKeyedScoped<IRepository<Status>, StatusRepository>("Status");
builder.Services.AddKeyedScoped<IRepository<Users>, UserRepository>("Users");

builder.Services.AddDbContext<TaskContext>(options =>
{
    options.UseSqlServer(builder.Configuration["MyDatabaseConnection"]); // utilizo un secreto para mi bd local

});



// Mappers
builder.Services.AddAutoMapper(typeof(Mapper), typeof(StatusMapper), typeof(UserMapper));

//Validadores
builder.Services.AddKeyedScoped<IValidator<InsertTaskDto>, TaskValidator>("TasksValidator");
builder.Services.AddKeyedScoped<IValidator<InsertStatusDto>, StatusValidator>("StatusValidator");
builder.Services.AddKeyedScoped<IValidator<UpdateTaskDto>, UpdateTaskValidator>("UpdateTasksValidator");
builder.Services.AddKeyedScoped<IValidator<UpdateStatusDto>, UpdateStatusValidator>("UpdateStatusValidator");


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(

    options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    }

    );


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Task Manager API",
        Description = "Una api para la gestion de tareas de forma colaborativa",
            
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskContext>();
    InitialSetup.Setup(context,builder.Configuration);

}
// Configure the HTTP request pipeline.

app.UseSwagger(c =>
{
    c.RouteTemplate = "Utils/swagger.json  ";
});
app.UseSwaggerUI();

app.Map("/swagger/v1/swagger.json", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.SendFileAsync("Utils/swagger.json");
    });
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("all");

app.Run();


