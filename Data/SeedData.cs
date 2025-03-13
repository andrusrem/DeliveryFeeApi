namespace DeliveryFeeApi.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            GenerateRegionalBaseFee(context);

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
    }
}
