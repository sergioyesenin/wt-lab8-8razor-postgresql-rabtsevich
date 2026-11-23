using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wt_lab8_8razor_postgresql_rabtsevich.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'100', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    IataCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Country = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    FoundedYear = table.Column<int>(type: "integer", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'100', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    SeatCapacity = table.Column<int>(type: "integer", nullable: false),
                    ManufactureYear = table.Column<int>(type: "integer", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    TailNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsOperational = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aircrafts_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'100', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Departure = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Destination = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SeatsAvailable = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    FlightType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Airlines",
                columns: new[] { "Id", "Country", "Description", "FoundedYear", "IataCode", "IsActive", "LogoUrl", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "Россия", "Крупнейшая авиакомпания России, участник SkyTeam.", 1923, "SU", true, null, "Аэрофлот", 4.7m },
                    { 2, "Россия", "Одна из ведущих частных авиакомпаний страны.", 1957, "S7", true, null, "S7 Airlines", 4.5m },
                    { 3, "ОАЭ", "Флагманская авиакомпания Дубая, крупнейший перевозчик Азии.", 1985, "EK", true, null, "Emirates", 4.9m },
                    { 4, "Германия", "Немецкий национальный перевозчик, основатель Star Alliance.", 1953, "LH", false, null, "Lufthansa", 4.4m }
                });

            migrationBuilder.InsertData(
                table: "Aircrafts",
                columns: new[] { "Id", "AirlineId", "IsOperational", "ManufactureYear", "Manufacturer", "Model", "SeatCapacity", "TailNumber" },
                values: new object[,]
                {
                    { 1, 1, true, 2018, "Airbus", "Airbus A320", 180, "RA-89001" },
                    { 2, 2, true, 2016, "Boeing", "Boeing 737-800", 189, "VQ-BRP" },
                    { 3, 3, true, 2014, "Airbus", "Airbus A380", 517, "A6-EVK" },
                    { 4, 4, false, 2012, "Boeing", "Boeing 747-8", 364, "D-ABYA" }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "AircraftId", "AirlineId", "Departure", "DepartureTime", "Destination", "FlightNumber", "FlightType", "IsActive", "SeatsAvailable", "TicketPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, "Москва", new DateTime(2025, 5, 10, 8, 30, 0, 0, DateTimeKind.Unspecified), "Санкт-Петербург", "SU-100", "Пассажирский", true, 120, 4500m },
                    { 2, 1, 1, "Москва", new DateTime(2025, 5, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), "Сочи", "SU-205", "Пассажирский", true, 45, 7800m },
                    { 3, 2, 2, "Казань", new DateTime(2025, 5, 12, 6, 15, 0, 0, DateTimeKind.Unspecified), "Новосибирск", "S7-310", "Пассажирский", false, 0, 6200m },
                    { 4, 3, 3, "Дубай", new DateTime(2025, 5, 13, 18, 45, 0, 0, DateTimeKind.Unspecified), "Москва", "EK-412", "Пассажирский", true, 78, 53000m },
                    { 5, 4, 4, "Франкфурт", new DateTime(2025, 5, 14, 21, 10, 0, 0, DateTimeKind.Unspecified), "Москва", "LH-518", "Пассажирский", true, 12, 41000m },
                    { 6, 1, 1, "Москва", new DateTime(2025, 5, 15, 11, 0, 0, 0, DateTimeKind.Unspecified), "Владивосток", "SU-777", "Пассажирский", true, 95, 18500m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AirlineId",
                table: "Aircrafts",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_TailNumber",
                table: "Aircrafts",
                column: "TailNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_IataCode",
                table: "Airlines",
                column: "IataCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightNumber",
                table: "Flights",
                column: "FlightNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
