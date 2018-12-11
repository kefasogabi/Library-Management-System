using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Data.Migrations
{
    public partial class addLibraryCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryId",
                table: "Patrons");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Patrons",
                newName: "LibraryCardId");

            migrationBuilder.RenameIndex(
                name: "IX_Patrons_LibraryId",
                table: "Patrons",
                newName: "IX_Patrons_LibraryCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryCardId",
                table: "Patrons");

            migrationBuilder.RenameColumn(
                name: "LibraryCardId",
                table: "Patrons",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Patrons_LibraryCardId",
                table: "Patrons",
                newName: "IX_Patrons_LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_LibraryCards_LibraryId",
                table: "Patrons",
                column: "LibraryId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
