using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Data.Migrations
{
    public partial class removebddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LocationId",
                table: "LibraryAssets");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "LibraryAssets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LocationId",
                table: "LibraryAssets",
                column: "LocationId",
                principalTable: "LibraryBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LocationId",
                table: "LibraryAssets");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "LibraryAssets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LocationId",
                table: "LibraryAssets",
                column: "LocationId",
                principalTable: "LibraryBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
