using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelingSalesmanWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartCityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EndCityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paths", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paths");
        }
    }
}
