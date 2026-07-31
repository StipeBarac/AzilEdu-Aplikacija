using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzilEdu.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDonors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonorStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    OrganizationName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DonorTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DonorStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donors_DonorStatuses_DonorStatusId",
                        column: x => x.DonorStatusId,
                        principalTable: "DonorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donors_DonorTypes_DonorTypeId",
                        column: x => x.DonorTypeId,
                        principalTable: "DonorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "DonorStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Novi" },
                    { 2, "Aktivan" },
                    { 3, "Povremeni" },
                    { 4, "Neaktivan" }
                });

            migrationBuilder.InsertData(
                table: "DonorTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fizička osoba" },
                    { 2, "Tvrtka" },
                    { 3, "Udruga ili organizacija" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donors_DonorStatusId",
                table: "Donors",
                column: "DonorStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Donors_DonorTypeId",
                table: "Donors",
                column: "DonorTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropTable(
                name: "DonorStatuses");

            migrationBuilder.DropTable(
                name: "DonorTypes");
        }
    }
}
