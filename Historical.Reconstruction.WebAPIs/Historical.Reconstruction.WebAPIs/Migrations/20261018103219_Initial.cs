using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Historical.Reconstruction.WebAPIs.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FindCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FindCategories_FindCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FindCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    RegionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FindCategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateFromYear = table.Column<int>(type: "INTEGER", nullable: false),
                    DateToYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    SourceUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sources_FindCategories_FindCategoryId",
                        column: x => x.FindCategoryId,
                        principalTable: "FindCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sources_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SourceImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FindId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StorageKey = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Caption = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceImages_Sources_FindId",
                        column: x => x.FindId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FindCategories_ParentId",
                table: "FindCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceImages_FindId",
                table: "SourceImages",
                column: "FindId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_FindCategoryId",
                table: "Sources",
                column: "FindCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_RegionId",
                table: "Sources",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourceImages");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "FindCategories");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
