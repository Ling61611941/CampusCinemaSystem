using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusCinema.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Director = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosterUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Actors", "Category", "Description", "Director", "Duration", "Name", "PosterUrl", "ReleaseDate", "Score", "Status" },
                values: new object[,]
                {
                    { 1, "吴京、刘德华、李雪健", "科幻", "太阳即将毁灭，人类在地球表面建造出巨大的推进器，寻找新的家园。", "郭帆", 173, "流浪地球2", "https://picsum.photos/300/420?random=1", new DateTime(2023, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 8.3m, "正在热映" },
                    { 2, "沈腾、易烊千玺、张译", "悬疑", "南宋绍兴年间，一场围绕密信展开的悬疑故事。", "张艺谋", 159, "满江红", "https://picsum.photos/300/420?random=2", new DateTime(2023, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 7.0m, "正在热映" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
