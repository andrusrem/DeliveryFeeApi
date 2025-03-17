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
            var base_fee_tallinn_car = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Car,
                StationName = StationEnum.Tallinn,
                Price = 4,
            };
            context.RegionalBaseFees.Add(base_fee_tallinn_car);

            var base_fee_tallinn_scooter = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Scooter,
                StationName = StationEnum.Tallinn,
                Price = 3.5M,
            };
            context.RegionalBaseFees.Add(base_fee_tallinn_scooter);

            var base_fee_tallinn_bike = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Bike,
                StationName = StationEnum.Tallinn,
                Price = 3,
            };
            context.RegionalBaseFees.Add(base_fee_tallinn_bike);

            //Base Fee for Tartu
            var base_fee_tartu_car = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Car,
                StationName = StationEnum.Tartu,
                Price = 3.5M,
            };
            context.RegionalBaseFees.Add(base_fee_tartu_car);
            var base_fee_tartu_scooter = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Scooter,
                StationName = StationEnum.Tartu,
                Price = 3,
            };
            context.RegionalBaseFees.Add(base_fee_tartu_scooter);
            var base_fee_tartu_bike = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Bike,
                StationName = StationEnum.Tartu,
                Price = 2.5M,
            };
            context.RegionalBaseFees.Add(base_fee_tartu_bike);

            //Base Fee for Pärnu
            var base_fee_parnu_car = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Car,
                StationName = StationEnum.Pärnu,
                Price = 3,
            };
            context.RegionalBaseFees.Add(base_fee_parnu_car);
            var base_fee_parnu_scooter = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Scooter,
                StationName = StationEnum.Pärnu,
                Price = 2.5M,
            };
            context.RegionalBaseFees.Add(base_fee_parnu_scooter);
            var base_fee_parnu_bike = new RegionalBaseFee
            {
                VehicleType = VehicleEnum.Bike,
                StationName = StationEnum.Pärnu,
                Price = 2,
            };
            context.RegionalBaseFees.Add(base_fee_parnu_bike);
        }

        private static void GenerateAirTemperatureExtraFees(ApplicationDbContext context)
        {
            if (context.AirTemperatureExtraFees.Count() > 0)
            {
                return;
            }

            //Extra fees for Scooter and Bike, when air temperature outside is less than -10;
            var air_extra_fee_less_than_minus_10_scooter = new AirTemperatureExtraFee
            {
                LowerTemperature = -273.15M,
                UpperTemperature = -10,
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.AirTemperatureExtraFees.Add(air_extra_fee_less_than_minus_10_scooter);

            var air_extra_fee_less_than_minus_10_bike = new AirTemperatureExtraFee
            {
                LowerTemperature = -273.15M,
                UpperTemperature = -10,
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.AirTemperatureExtraFees.Add(air_extra_fee_less_than_minus_10_bike);

            //Extra fees for Scooter and Bike, when air temperature outside is less than 0 but more that -10;
            var air_extra_fee_less_than_0_but_more_than_minus_10_scooter = new AirTemperatureExtraFee
            {
                LowerTemperature = -10,
                UpperTemperature = 0,
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.AirTemperatureExtraFees.Add(air_extra_fee_less_than_0_but_more_than_minus_10_scooter);

            var air_extra_fee_less_than_0_but_more_than_minus_10_bike = new AirTemperatureExtraFee
            {
                LowerTemperature = -10,
                UpperTemperature = 0,
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.AirTemperatureExtraFees.Add(air_extra_fee_less_than_0_but_more_than_minus_10_bike);
        }

        private static void GenerateWindSpeedExtraFees(ApplicationDbContext context)
        {
            if(context.WindSpeedExtraFees.Count() > 0)
            {
                return;
            }

            //Wind extra fee is paid in case vehicle type is Bike
            var wind_extra_fee_between_10_and_20_m_s = new WindSpeedExtraFee
            {
                LowerSpeed = 10,
                UpperSpeed = 20,
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WindSpeedExtraFees.Add(wind_extra_fee_between_10_and_20_m_s);

            var wind_extra_fee_greater_than_20_m_s = new WindSpeedExtraFee
            {
                LowerSpeed = 20,
                UpperSpeed = 1000,
                VehicleType = VehicleEnum.Bike,
                Forbitten = true,
            };
            context.WindSpeedExtraFees.Add(wind_extra_fee_greater_than_20_m_s);
        }

        private static void GenerateWeatherPhenomenonExtraFee(ApplicationDbContext context)
        {
            if(context.WeatherPhenomenonExtraFees.Count() > 0)
            {
                return;
            }
            //!!!RAIN RELATED 0.5 euro:

            //Light rain
            var phenomenon_extra_fee_light_rain_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light rain",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_rain_scooter);

            var phenomenon_extra_fee_light_rain_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light rain",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_rain_bike);

            //Moderate rain
            var phenomenon_extra_fee_moderate_rain_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate rain",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_rain_scooter);

            var phenomenon_extra_fee_moderate_rain_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate rain",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_rain_bike);

            //Heavy rain
            var phenomenon_extra_fee_heavy_rain_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy rain",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_rain_scooter);

            var phenomenon_extra_fee_heavy_rain_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy rain",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_rain_bike);

            //Light shower
            var phenomenon_extra_fee_light_shower_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_shower_scooter);

            var phenomenon_extra_fee_light_shower_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light shower",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_shower_bike);

            //Moderate shower
            var phenomenon_extra_fee_moderate_shower_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_shower_scooter);

            var phenomenon_extra_fee_moderate_shower_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate shower",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_shower_bike);

            //Heavy shower
            var phenomenon_extra_fee_heavy_shower_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_shower_scooter);

            var phenomenon_extra_fee_heavy_shower_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy shower",
                VehicleType = VehicleEnum.Bike,
                Price = 0.5M,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_shower_bike);

            //!!!SNOW OR SLEET RELATED 1 euro:

            //Light snowfall
            var phenomenon_extra_fee_light_snowfall_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snowfall",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_snowfall_scooter);

            var phenomenon_extra_fee_light_snowfall_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snowfall",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_snowfall_bike);

            //Moderate snowfall
            var phenomenon_extra_fee_moderate_snowfall_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snowfall",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_snowfall_scooter);

            var phenomenon_extra_fee_moderate_snowfall_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snowfall",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_snowfall_bike);

            //Heavy snowfall
            var phenomenon_extra_fee_heavy_snowfall_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snowfall",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_snowfall_scooter);

            var phenomenon_extra_fee_heavy_snowfall_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snowfall",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_snowfall_bike);

            //Light snow shower
            var phenomenon_extra_fee_light_snow_shower_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snow shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_snow_shower_scooter);

            var phenomenon_extra_fee_light_snow_shower_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light snow shower",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_snow_shower_bike);

            //Moderate snow shower
            var phenomenon_extra_fee_moderate_snow_shower_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snow shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_snow_shower_scooter);

            var phenomenon_extra_fee_moderate_snow_shower_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate snow shower",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_snow_shower_bike);

            //Heavy snow shower
            var phenomenon_extra_fee_heavy_snow_shower_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snow shower",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_snow_shower_scooter);

            var phenomenon_extra_fee_heavy_snow_shower_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Heavy snow shower",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_heavy_snow_shower_bike);

            //Light sleet
            var phenomenon_extra_fee_light_sleet_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light sleet",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_sleet_scooter);

            var phenomenon_extra_fee_light_sleet_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Light sleet",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_light_sleet_bike);

            //Moderate sleet
            var phenomenon_extra_fee_moderate_sleet_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate sleet",
                VehicleType = VehicleEnum.Scooter,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_sleet_scooter);

            var phenomenon_extra_fee_moderate_sleet_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Moderate sleet",
                VehicleType = VehicleEnum.Bike,
                Price = 1,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_moderate_sleet_bike);

            // WHEN GLAZE, HAIL AND THUNDER “Usage of selected vehicle type is forbidden”
            //Glaze
            var phenomenon_extra_fee_glaze_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Glaze",
                VehicleType = VehicleEnum.Scooter,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_glaze_scooter);

            var phenomenon_extra_fee_glaze_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Glaze",
                VehicleType = VehicleEnum.Bike,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_glaze_bike);

            //Hail
            var phenomenon_extra_fee_hail_scooter = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Hail",
                VehicleType = VehicleEnum.Scooter,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_hail_scooter);

            var phenomenon_extra_fee_hail_bike = new WeatherPhenomenonExtraFee
            {
                WeatherPhenomenon = "Hail",
                VehicleType = VehicleEnum.Bike,
                Forbitten = true,
            };
            context.WeatherPhenomenonExtraFees.Add(phenomenon_extra_fee_hail_bike);
        }

    }
}
