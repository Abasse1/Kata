using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Serenza.Sensor.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    SensorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    TemperatureMax = table.Column<double>(type: "REAL", nullable: false),
                    TemperatureMin = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.SensorId);
                });

            migrationBuilder.CreateTable(
                name: "SensorHistories",
                columns: table => new
                {
                    SensorHistoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    GetTemperatureDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    SensorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorHistories", x => x.SensorHistoryId);
                    table.ForeignKey(
                        name: "FK_SensorHistories_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "SensorId", "State", "TemperatureMax", "TemperatureMin" },
                values: new object[,]
                {
                    { 1, "COLD", 22.0, -32768.0 },
                    { 2, "WARM", 40.0, 22.0 },
                    { 3, "HOT", 32767.0, 40.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorHistories_SensorId",
                table: "SensorHistories",
                column: "SensorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorHistories");

            migrationBuilder.DropTable(
                name: "Sensors");
        }
    }
}
