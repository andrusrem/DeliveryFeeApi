using DeliveryFeeApi.CronJobs;
using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Project") ??  "Data Source=Project.db";

builder.Services.AddSqlite<ApplicationDbContext>(connectionString);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStationWeatherService, StationWeatherService>();
builder.Services.AddScoped<IStationWeatherRepository, StationWeatherRepository>();

builder.Services.AddHttpClient();


builder.Services.AddQuartz(options =>
{
    var jobKey = JobKey.Create("LoadWeatherJob");
    options.AddJob<LoadStationWeatherJob>(jobKey)
        .AddTrigger(trigger =>
        trigger
            .ForJob(jobKey)
            .StartAt(DateTime.Now.AddSeconds(5))
            .WithSimpleSchedule(time => time.WithIntervalInHours(1).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
