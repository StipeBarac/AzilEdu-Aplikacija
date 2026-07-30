using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzilEdu.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VolunteerStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Skills = table.Column<string>(type: "TEXT", nullable: false),
                    AvailableFrom = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    VolunteerStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Volunteers_VolunteerStatuses_VolunteerStatusId",
                        column: x => x.VolunteerStatusId,
                        principalTable: "VolunteerStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "VolunteerStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Novi" },
                    { 2, "Aktivan" },
                    { 3, "Privremeno nedostupan" },
                    { 4, "Neaktivan" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_VolunteerStatusId",
                table: "Volunteers",
                column: "VolunteerStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "VolunteerStatuses");
        }
    }
}
