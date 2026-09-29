using Microsoft.EntityFrameworkCore;
using OpsControl.Application.Incidents.Repositories;
using OpsControl.Application.Incidents.UseCases;
using OpsControl.Application.UseCase;
using OpsControl.Infrastructure.Data;
using OpsControl.Infrastructure.Incidents.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OpsControl");
// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(allowIntegerValues: false));
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<ResolveIncident>();
builder.Services.AddScoped<GetIncidentById>();
builder.Services.AddScoped<GetIncidents>();
builder.Services.AddScoped<CreateIncident>();
builder.Services.AddScoped<UpdateIncident>();
builder.Services.AddScoped<DeleteIncident>();
builder.Services.AddScoped<StartIncident>();
builder.Services.AddScoped<GetIncidentSummary>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactLocal", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors("ReactLocal");

app.UseAuthorization();

app.MapControllers();

app.Run();
