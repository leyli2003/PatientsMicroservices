using Patients.APP.Domain;
using Patients.APP.Features.Patients;
using Microsoft.EntityFrameworkCore;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString(nameof(PatientsDb));
builder.Services.AddDbContext<DbContext, PatientsDb>(options => options.UseSqlite(connectionString));

// For Mediator Injection
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(PatientQueryRequest).Assembly));

builder.Services.AddControllers();
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