using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SecurityNews.Migrations
{
    public partial class ADDSpiderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Abstract",
                table: "News_Tbl",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 400);

            migrationBuilder.CreateTable(
                name: "MainLink_tbl",
                columns: table => new
                {
                    MainLinkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateNews = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Titel = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    XpathContent = table.Column<string>(nullable: true),
                    XpathTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainLink_tbl", x => x.MainLinkId);
                });

            migrationBuilder.CreateTable(
                name: "InternalLink_tbl",
                columns: table => new
                {
                    InternalLinkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IntrnalUrl = table.Column<string>(nullable: true),
                    MainlinkID = table.Column<int>(nullable: false),
                    SpiderDate = table.Column<DateTime>(nullable: false),
                    SpiderStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalLink_tbl", x => x.InternalLinkId);
                    table.ForeignKey(
                        name: "FK_InternalLink_tbl_MainLink_tbl_MainlinkID",
                        column: x => x.MainlinkID,
                        principalTable: "MainLink_tbl",
                        principalColumn: "MainLinkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsSpider_tbl",
                columns: table => new
                {
                    IdNews = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    IntrnalLinkID = table.Column<int>(nullable: false),
                    NewsDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Titel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsSpider_tbl", x => x.IdNews);
                    table.ForeignKey(
                        name: "FK_NewsSpider_tbl_InternalLink_tbl_IntrnalLinkID",
                        column: x => x.IntrnalLinkID,
                        principalTable: "InternalLink_tbl",
                        principalColumn: "InternalLinkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalLink_tbl_MainlinkID",
                table: "InternalLink_tbl",
                column: "MainlinkID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsSpider_tbl_IntrnalLinkID",
                table: "NewsSpider_tbl",
                column: "IntrnalLinkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsSpider_tbl");

            migrationBuilder.DropTable(
                name: "InternalLink_tbl");

            migrationBuilder.DropTable(
                name: "MainLink_tbl");

            migrationBuilder.AlterColumn<string>(
                name: "Abstract",
                table: "News_Tbl",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
