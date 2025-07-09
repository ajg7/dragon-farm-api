using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DragonFarmApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dragons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    HatchedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RarityScore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dragons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DominantAllele = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    RecessiveAllele = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DragonTraits",
                columns: table => new
                {
                    DragonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TraitId = table.Column<int>(type: "int", nullable: false),
                    AlleleA = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    AlleleB = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DragonTraits", x => new { x.DragonId, x.TraitId });
                    table.ForeignKey(
                        name: "FK_DragonTraits_Dragons_DragonId",
                        column: x => x.DragonId,
                        principalTable: "Dragons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DragonTraits_Traits_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Traits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Dragons",
                columns: new[] { "Id", "HatchedAt", "Name", "RarityScore", "Sex" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Pyro", 0.0, 1 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Astra", 0.0, 2 }
                });

            migrationBuilder.InsertData(
                table: "Traits",
                columns: new[] { "Id", "DominantAllele", "Name", "RecessiveAllele" },
                values: new object[,]
                {
                    { 1, "R", "Color", "r" },
                    { 2, "W", "WingSpan", "w" },
                    { 3, "S", "Claw", "s" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DragonTraits_TraitId",
                table: "DragonTraits",
                column: "TraitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DragonTraits");

            migrationBuilder.DropTable(
                name: "Dragons");

            migrationBuilder.DropTable(
                name: "Traits");
        }
    }
}
