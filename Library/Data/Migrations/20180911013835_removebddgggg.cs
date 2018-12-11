using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Data.Migrations
{
    public partial class removebddgggg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryCardId",
                table: "Patrons",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryCardId",
                table: "Patrons",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
