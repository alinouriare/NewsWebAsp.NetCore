using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SecurityNews.Migrations
{
    public partial class Create2poll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Poll_Tbl",
                columns: table => new
                {
                    PollId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    PollEndDate = table.Column<string>(nullable: true),
                    PollStartDate = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poll_Tbl", x => x.PollId);
                });

            migrationBuilder.CreateTable(
                name: "Polloption_Tbl",
                columns: table => new
                {
                    PolloptionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Answer = table.Column<string>(nullable: true),
                    PollID = table.Column<int>(nullable: false),
                    VouteCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polloption_Tbl", x => x.PolloptionID);
                    table.ForeignKey(
                        name: "FK_Polloption_Tbl_Poll_Tbl_PollID",
                        column: x => x.PollID,
                        principalTable: "Poll_Tbl",
                        principalColumn: "PollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polloption_Tbl_PollID",
                table: "Polloption_Tbl",
                column: "PollID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Polloption_Tbl");

            migrationBuilder.DropTable(
                name: "Poll_Tbl");
        }
    }
}
