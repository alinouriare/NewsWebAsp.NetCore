using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SecurityNews.Migrations
{
    public partial class NewTableComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments_Tbl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DisLikeCount = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(maxLength: 60, nullable: false),
                    IP = table.Column<string>(nullable: true),
                    LikeCount = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 2000, nullable: false),
                    NewsId = table.Column<int>(nullable: false),
                    ReplyID = table.Column<int>(nullable: false),
                    commentDate = table.Column<string>(nullable: true),
                    commentTime = table.Column<string>(nullable: true),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments_Tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Tbl_News_Tbl_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News_Tbl",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Tbl_NewsId",
                table: "Comments_Tbl",
                column: "NewsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments_Tbl");
        }
    }
}
