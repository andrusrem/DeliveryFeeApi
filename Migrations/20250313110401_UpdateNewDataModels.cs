using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryFeeApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewDataModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WeatherPhenomenon",
                table: "WeatherPhenomenonExtraFees",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WeatherPhenomenon",
                table: "WeatherPhenomenonExtraFees",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
