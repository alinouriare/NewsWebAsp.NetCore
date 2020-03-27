using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SecurityNews.Migrations
{
    public partial class CreateDBnewscategorys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryImpact_Tbl",
                columns: table => new
                {
                    CategoryImpactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 150, nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryImpact_Tbl", x => x.CategoryImpactId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPlatform_Tbl",
                columns: table => new
                {
                    CategoryPlatformtId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 150, nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPlatform_Tbl", x => x.CategoryPlatformtId);
                });

            migrationBuilder.CreateTable(
                name: "News_Tbl",
                columns: table => new
                {
                    NewsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abstract = table.Column<string>(maxLength: 400, nullable: false),
                    CVE = table.Column<string>(nullable: true),
                    CategoryImpactID = table.Column<int>(nullable: false),
                    CategoryPlatformID = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    IndexImage = table.Column<string>(nullable: true),
                    NewsDate = table.Column<string>(nullable: true),
                    NewsTime = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    VisitCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News_Tbl", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_News_Tbl_CategoryImpact_Tbl_CategoryImpactID",
                        column: x => x.CategoryImpactID,
                        principalTable: "CategoryImpact_Tbl",
                        principalColumn: "CategoryImpactId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_News_Tbl_CategoryPlatform_Tbl_CategoryPlatformID",
                        column: x => x.CategoryPlatformID,
                        principalTable: "CategoryPlatform_Tbl",
                        principalColumn: "CategoryPlatformtId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_News_Tbl_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_Tbl_CategoryImpactID",
                table: "News_Tbl",
                column: "CategoryImpactID");

            migrationBuilder.CreateIndex(
                name: "IX_News_Tbl_CategoryPlatformID",
                table: "News_Tbl",
                column: "CategoryPlatformID");

            migrationBuilder.CreateIndex(
                name: "IX_News_Tbl_UserID",
                table: "News_Tbl",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News_Tbl");

            migrationBuilder.DropTable(
                name: "CategoryImpact_Tbl");

            migrationBuilder.DropTable(
                name: "CategoryPlatform_Tbl");
        }
    }
}
