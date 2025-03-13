using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryFeeApi.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250313150904_DeleteStationEnumValueFromExtraFeeModels")]
    public partial class DeleteStationEnumValueFromExtraFeeModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("DeliveryFeeApi.Data.AirTemperatureExtraFee", b =>
            {
                b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                b.Property<decimal>("LowerTemperature")
                    .HasColumnType("TEXT");

                b.Property<decimal>("Price")
                    .HasColumnType("TEXT");

                b.Property<decimal>("UpperTemperature")
                    .HasColumnType("TEXT");

                b.Property<int>("VehicleType")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.ToTable("AirTemperatureExtraFees");
            });

            modelBuilder.Entity("DeliveryFeeApi.Data.WeatherPhenomenonExtraFee", b =>
            {
                b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                b.Property<decimal?>("Price")
                    .HasColumnType("TEXT");

                b.Property<int>("VehicleType")
                    .HasColumnType("INTEGER");

                b.Property<string>("WeatherPhenomenon")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("WeatherPhenomenonExtraFees");
            });

            modelBuilder.Entity("DeliveryFeeApi.Data.WindSpeedExtraFee", b =>
            {
                b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                b.Property<decimal>("LowerSpeed")
                    .HasColumnType("TEXT");

                b.Property<decimal?>("Price")
                    .HasColumnType("TEXT");

                b.Property<decimal?>("UpperSpeed")
                    .HasColumnType("TEXT");

                b.Property<int>("VehicleType")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.ToTable("WindSpeedExtraFees");
            });
        }
    }
}
