using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
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
// Add services to the container.

builder.Services.AddControllers();

//Servicios
builder.Services.AddKeyedScoped<ICommonService<TaskDto, InsertTaskDto, UpdateTaskDto, int>, TaskService>("TaskService");
builder.Services.AddKeyedScoped <ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int>,StatusService>("StatusService");
builder.Services.AddScoped<IUserService, UsersService>();

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
        options.ExpireTimeSpan = TimeSpan.FromSeconds(40);
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    }

    );


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("all");

app.Run();


