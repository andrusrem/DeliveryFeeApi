using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryFeeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNewDataModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirTemperatureExtraFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LowerTemperature = table.Column<decimal>(type: "TEXT", nullable: false),
                    UpperTemperature = table.Column<decimal>(type: "TEXT", nullable: false),
                    VehicleType = table.Column<int>(type: "INTEGER", nullable: false),
                    StationName = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirTemperatureExtraFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalBaseFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleType = table.Column<int>(type: "INTEGER", nullable: false),
                    StationName = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalBaseFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherPhenomenonExtraFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeatherPhenomenon = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleType = table.Column<int>(type: "INTEGER", nullable: false),
                    StationName = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherPhenomenonExtraFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WindSpeedExtraFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LowerSpeed = table.Column<decimal>(type: "TEXT", nullable: false),
                    UpperSpeed = table.Column<decimal>(type: "TEXT", nullable: false),
                    VehicleType = table.Column<int>(type: "INTEGER", nullable: false),
                    StationName = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WindSpeedExtraFees", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirTemperatureExtraFees");

            migrationBuilder.DropTable(
                name: "RegionalBaseFees");

            migrationBuilder.DropTable(
                name: "WeatherPhenomenonExtraFees");

            migrationBuilder.DropTable(
                name: "WindSpeedExtraFees");
        }
    }
}
