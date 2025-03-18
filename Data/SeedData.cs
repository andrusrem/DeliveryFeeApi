using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Data
{
    [ExcludeFromCodeCoverage]
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            GenerateRegionalBaseFee(context);
            GenerateAirTemperatureExtraFees(context);
            GenerateWindSpeedExtraFees(context);
            GenerateWeatherPhenomenonExtraFee(context);

            context.SaveChanges();
        }

        private static void GenerateRegionalBaseFee(ApplicationDbContext context)
        {
            if(context.RegionalBaseFees.Count() > 0)
            {
                return;
            }
            //Base Fee for Tallinn
            var baseFeeTallinnCar = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Car,
                StationName = StationEnum.Tallinn,
                Price = 4,
            };
            context.RegionalBaseFees.Add(baseFeeTallinnCar);

            var baseFeeTallinnScooter = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Scooter,
                StationName = StationEnum.Tallinn,
                Price = 3.5M,
            };
            context.RegionalBaseFees.Add(baseFeeTallinnScooter);

            var baseFeeTallinnBike = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Bike,
                StationName = StationEnum.Tallinn,
                Price = 3,
            };
            context.RegionalBaseFees.Add(baseFeeTallinnBike);

            //Base Fee for Tartu
            var baseFeeTartuCar = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Car,
                StationName = StationEnum.Tartu,
                Price = 3.5M,
            };
            context.RegionalBaseFees.Add(baseFeeTartuCar);

            var baseFeeTartuScooter = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Scooter,
                StationName = StationEnum.Tartu,
                Price = 3,
            };
            context.RegionalBaseFees.Add(baseFeeTartuScooter);

            var baseFeeTartuBike = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Bike,
                StationName = StationEnum.Tartu,
                Price = 2.5M,
            };
            context.RegionalBaseFees.Add(baseFeeTartuBike);

            //Base Fee for Pärnu
            var baseFeeParnuCar = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Car,
                StationName = StationEnum.Pärnu,
                Price = 3,
            };
            context.RegionalBaseFees.Add(baseFeeParnuCar);
            var baseFeeParnuScooter = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Scooter,
                StationName = StationEnum.Pärnu,
                Price = 2.5M,
            };
            context.RegionalBaseFees.Add(baseFeeParnuScooter);
            var baseFeeParnuBike = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Bike,
                StationName = StationEnum.Pärnu,
                Price = 2,
            };
            context.RegionalBaseFees.Add(baseFeeParnuBike);
        }

        private static void GenerateAirTemperatureExtraFees(ApplicationDbContext context)
        {
            if (context.AirTemperatureExtraFees.Count() > 0)
            {
                return;
            }

            //Extra fees for Scooter and Bike, when air temperature outside is less than -10;
            var airExtraFeeLessThanMinus10Scooter = new AirTemperatureExtraFee
            {
                LowerTemperature = -273.15M,
                UpperTemperature = -10,
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.AirTemperatureExtraFees.Add(airExtraFeeLessThanMinus10Scooter);

            var airExtraFeeLessThanMinus10Bike = new AirTemperatureExtraFee
            {
                LowerTemperature = -273.15M,
                UpperTemperature = -10,
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.AirTemperatureExtraFees.Add(airExtraFeeLessThanMinus10Bike);

            //Extra fees for Scooter and Bike, when air temperature outside is less than 0 but more that -10;
            var airExtraFeeLessThan0ButMoreThanMinus10Scooter = new AirTemperatureExtraFee
            {
                LowerTemperature = -10,
                UpperTemperature = 0,
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.AirTemperatureExtraFees.Add(airExtraFeeLessThan0ButMoreThanMinus10Scooter);

            var airExtraFeeLessThan0ButMoreThanMinus10Bike = new AirTemperatureExtraFee
            {
                LowerTemperature = -10,
                UpperTemperature = 0,
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.AirTemperatureExtraFees.Add(airExtraFeeLessThan0ButMoreThanMinus10Bike);
        }

        private static void GenerateWindSpeedExtraFees(ApplicationDbContext context)
        {
            if(context.WindSpeedExtraFees.Count() > 0)
            {
                return;
            }

            //Wind extra fee is paid in case vehicle type is Bike
            var windExtraFeeBetween10And20MS = new WindSpeedExtraFee
            {
                LowerSpeed = 10,
                UpperSpeed = 20,
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WindSpeedExtraFees.Add(windExtraFeeBetween10And20MS);

            var windExtraFeeGreaterThan20MS = new WindSpeedExtraFee
            {
                LowerSpeed = 20,
                UpperSpeed = 1000,
                VehicleType = VehicleEnum.Bike,
                Forbitten = true,
            };
            context.WindSpeedExtraFees.Add(windExtraFeeGreaterThan20MS);
        }

        private static void GenerateWeatherPhenomenonExtraFee(ApplicationDbContext context)
        {
            if(context.WeatherPhenomenonExtraFees.Count() > 0)
            {
                return;
            }
            //!!!RAIN RELATED 0.5 euro:

            //Light rain
            var phenomenonExtraFeeLightRainScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light rain",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightRainScooter);

            var phenomenonExtraFeeLightRainBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light rain",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightRainBike);

            //Moderate rain
            var phenomenonExtraFeeModerateRainScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate rain",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateRainScooter);

            var phenomenonExtraFeeModerateRainBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate rain",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateRainBike);

            //Heavy rain
            var phenomenonExtraFeeHeavyRainScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy rain",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavyRainScooter);

            var phenomenonExtraFeeHeavyRainBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy rain",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavyRainBike);

            //Light shower
            var phenomenonExtraFeeLightShowerScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightShowerScooter);

            var phenomenonExtraFeeLightShowerBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light shower",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightShowerBike);

            //Moderate shower
            var phenomenonExtraFeeModerateShowerScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateShowerScooter);

            var phenomenonExtraFeeModerateShowerBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate shower",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateShowerBike);

            //Heavy shower
            var phenomenonExtraFeeHeavyShowerScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavyShowerScooter);

            var phenomenonExtraFeeHeavyShowerBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy shower",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavyShowerBike);

            //!!!SNOW OR SLEET RELATED 1 euro:

            //Light snowfall
            var phenomenonExtraFeeLightSnowfallScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snowfall",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightSnowfallScooter);

            var phenomenonExtraFeeLightSnowfallBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snowfall",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightSnowfallBike);

            //Moderate snowfall
            var phenomenonExtraFeeModerateSnowfallScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snowfall",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateSnowfallScooter);

            var phenomenonExtraFeeModerateSnowfallBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snowfall",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateSnowfallBike);

            //Heavy snowfall
            var phenomenonExtraFeeHeavySnowfallScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snowfall",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavySnowfallScooter);

            var phenomenonExtraFeeHeavySnowfallBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snowfall",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavySnowfallBike);

            // Light snow shower
            var phenomenonExtraFeeLightSnowShowerScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snow shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightSnowShowerScooter);

            var phenomenonExtraFeeLightSnowShowerBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snow shower",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightSnowShowerBike);

            // Moderate snow shower
            var phenomenonExtraFeeModerateSnowShowerScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snow shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateSnowShowerScooter);

            var phenomenonExtraFeeModerateSnowShowerBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snow shower",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateSnowShowerBike);

            // Heavy snow shower
            var phenomenonExtraFeeHeavySnowShowerScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snow shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavySnowShowerScooter);

            var phenomenonExtraFeeHeavySnowShowerBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snow shower",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHeavySnowShowerBike);

            // Light sleet
            var phenomenonExtraFeeLightSleetScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light sleet",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightSleetScooter);

            var phenomenonExtraFeeLightSleetBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light sleet",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeLightSleetBike);

            // Moderate sleet
            var phenomenonExtraFeeModerateSleetScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate sleet",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateSleetScooter);

            var phenomenonExtraFeeModerateSleetBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate sleet",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeModerateSleetBike);

            // WHEN GLAZE, HAIL AND THUNDER “Usage of selected vehicle type is forbidden”
            // Glaze
            var phenomenonExtraFeeGlazeScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Glaze",
                VehicleType = VehicleEnum.Scooter,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeGlazeScooter);

            var phenomenonExtraFeeGlazeBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Glaze",
                VehicleType = VehicleEnum.Bike,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeGlazeBike);

            // Hail
            var phenomenonExtraFeeHailScooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Hail",
                VehicleType = VehicleEnum.Scooter,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHailScooter);

            var phenomenonExtraFeeHailBike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Hail",
                VehicleType = VehicleEnum.Bike,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenonExtraFeeHailBike);
        }

    }
}
