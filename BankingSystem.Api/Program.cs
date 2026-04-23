/*
 * Name: Anamika Rawat
 * StudentID: 9087089
 */
using BankingSystem.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext to use the same SQLite database as the Web project
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BankingDb")));

// Register the Repository for Dependency Injection
builder.Services.AddScoped<IBankRepository, BankRepository>();

// Configure Swagger for API documentation
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
