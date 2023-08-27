using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Регистрация контекста
builder.Services.AddDbContext<CustomerContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("db"));
    x.UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped(typeof(DbContext), typeof(CustomerContext));
builder.Services.AddScoped(typeof(DbConnection), (_) => new NpgsqlConnection(builder.Configuration.GetConnectionString("db")));

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
