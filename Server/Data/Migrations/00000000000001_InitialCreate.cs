using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tripbuk.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "LocationCenters",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParentTags",
                schema: "public",
                columns: table => new
                {
                    ParentTagId = table.Column<int>(type: "integer", nullable: false),
                    ChildTagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PlaceTags",
                schema: "public",
                columns: table => new
                {
                    PlaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagName = table.Column<string>(type: "text", nullable: false),
                    AllNamesByLocale = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ParentDestinationId = table.Column<int>(type: "integer", nullable: true),
                    LookupId = table.Column<string>(type: "text", nullable: true),
                    DestinationUrl = table.Column<string>(type: "text", nullable: true),
                    DefaultCurrencyCode = table.Column<string>(type: "text", nullable: true),
                    TimeZone = table.Column<string>(type: "text", nullable: true),
                    IATACodes = table.Column<List<string>>(type: "text[]", nullable: true),
                    CountryCallingCode = table.Column<string>(type: "text", nullable: true),
                    Languages = table.Column<List<string>>(type: "text[]", nullable: true),
                    LocationCenterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_LocationCenters_LocationCenterId",
                        column: x => x.LocationCenterId,
                        principalSchema: "public",
                        principalTable: "LocationCenters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Places",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Center = table.Column<string>(type: "text", nullable: true),
                    DestinationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalSchema: "public",
                        principalTable: "Destinations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_LocationCenterId",
                schema: "public",
                table: "Destinations",
                column: "LocationCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_DestinationId",
                schema: "public",
                table: "Places",
                column: "DestinationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentTags",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Places",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PlaceTags",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Destinations",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LocationCenters",
                schema: "public");
        }
    }
}
