using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace DeliveryFeeApi.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class DeleteStationEnumValueFromExtraFeeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationName",
                table: "WindSpeedExtraFees");

            migrationBuilder.DropColumn(
                name: "StationName",
                table: "WeatherPhenomenonExtraFees");

            migrationBuilder.DropColumn(
                name: "StationName",
                table: "AirTemperatureExtraFees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationName",
                table: "WindSpeedExtraFees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationName",
                table: "WeatherPhenomenonExtraFees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationName",
                table: "AirTemperatureExtraFees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
