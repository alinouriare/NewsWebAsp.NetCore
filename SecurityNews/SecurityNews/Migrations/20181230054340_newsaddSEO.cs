using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SecurityNews.Migrations
{
    public partial class newsaddSEO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "News_Tbl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTag",
                table: "News_Tbl",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "News_Tbl");

            migrationBuilder.DropColumn(
                name: "MetaTag",
                table: "News_Tbl");
        }
    }
}
