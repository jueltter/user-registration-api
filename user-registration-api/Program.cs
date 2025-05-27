using FluentValidation;
using Microsoft.EntityFrameworkCore;
using user_registration_api.DefaultDomain.Data;
using user_registration_api.DefaultDomain.Repositories;
using user_registration_api.DefaultDomain.Repositories.Impl;
using user_registration_api.DefaultDomain.Services;
using user_registration_api.DefaultDomain.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJsonService, JsonService>();

builder.Services.AddControllers();

// model validators configuration
foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) builder.Services.AddValidatorsFromAssembly(assembly);

// automapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContextFactory<CustomDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("CustomDbContext"));
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();
