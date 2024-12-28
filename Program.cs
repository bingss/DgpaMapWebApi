using DgpaMapWebApi.DaliyProcess;
using DgpaMapWebApi.Interface;
using DgpaMapWebApi.Models;
using DgpaMapWebApi.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DgpaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DgpaDatabase")));

//初始化或更新資料--暫時--
//builder.Services.Configure<HereMapSetting>(builder.Configuration.GetSection("HereMapSetting"));

//注入service
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IUpdateDateService, UpdateDateService>();

var app = builder.Build();

//Scaffold -DbContext "Server=127.0.0.1;Database=DgpaDb;User ID=dcpa;Password=123456;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
//Scaffold-DbContext "Server=127.0.0.1;Database=DgpaDb;User ID=dcpa;Password=123456;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -NoOnConfiguring -UseDatabaseNames -NoPluralize -Force
//初始化或更新資料--暫時--
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    DailyProcess.Initialize(services);
//    DailyProcess.UpdateJob(services);
//    DailyProcess.MoveExpiredJob(services);
//}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    //app.UseSwagger();
    //app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
