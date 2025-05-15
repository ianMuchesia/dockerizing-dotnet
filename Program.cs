using Todo.AppDbContext;
using Todo.Application;
using Todo.Contracts.Interfaces;
using Todo.Middleware;
using Todo.Models;
using Todo.Repository.Implementations;
using Todo.Repository.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddLogging();

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
builder.Services.AddSingleton<TodoDbContext>();

// Register repositories and services
// Add repositories
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// { 
//     using var scope = app.Services.CreateScope();
//     var context = scope.ServiceProvider;
// }

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseExceptionHandler();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();