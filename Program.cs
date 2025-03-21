using DeliveryFeeApi.CronJobs;
using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi
{
    public class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("Project") ?? "Data Source=Project.db";

            builder.Services.AddSqlite<ApplicationDbContext>(connectionString);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IStationWeatherService, StationWeatherService>();
            builder.Services.AddScoped<IDeliveryPriceService, DeliveryPriceService>();
            builder.Services.AddScoped<IAirTemperatureExtraFeeService, AirTemperatureExtraFeeService>();
            builder.Services.AddScoped<IRegionalBaseFeeService, RegionalBaseFeeService>();
            builder.Services.AddScoped<IWindSpeedExtraFeeService, WindSpeedExtraFeeService>();
            builder.Services.AddScoped<IWeatherPhenomenonExtraFeeService, WeatherPhenomenonExtraFeeService>();

            builder.Services.AddScoped<IStationWeatherRepository, StationWeatherRepository>();
            builder.Services.AddScoped<IRegionalBaseFeeRepository, RegionalBaseFeeRepository>();
            builder.Services.AddScoped<IWindSpeedExtraFeeRepository, WindSpeedExtraFeeRepository>();
            builder.Services.AddScoped<IWeatherPhenomenonExtraFeeRepository, WeatherPhenomenonExtraFeeRepository>();
            builder.Services.AddScoped<IAirTemperatureExtraFeeRepository, AirTemperatureExtraFeeRepository>();

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

            //Add Auto-Migration functionality
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                for (int i = 0; i < 10; i++) // Retry loop for database readiness
                {
                    try
                    {
                        dbContext.Database.Migrate();
                        break; // Break if migration is successful
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Migration attempt {i + 1} failed: {ex.Message}");
                        Thread.Sleep(5000); // Wait 5 seconds before retrying
                    }
                }
            }

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

            //Add Seeder for Extra fee seeding
            using (var scope = app.Services.CreateScope())
            {
                var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedData.Generate(applicationDbContext);

            }

            app.Run();
        }
    }
}