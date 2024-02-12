using BankManagment_Infrastructure.Context;
using BankManagment_Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using BankManagment_DependencyInjectionExtensions;
using Serilog;
using Serilog.Events;
using BankManagment_Services;
using AutoMapper;
using BankManagment_Mapper;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration).WriteTo.Console().WriteTo.File("BankManagment-.txt"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("BankManagment_Migration");
        });
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
//builder.Services.RegisterServicesAndRepositories(Assembly.GetAssembly(typeof(IAccountTypeService)));
builder.Services.RegisterServicesAndRepositories();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
        app.UseMiddleware<ExceptionHandlerMiddleware>();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
