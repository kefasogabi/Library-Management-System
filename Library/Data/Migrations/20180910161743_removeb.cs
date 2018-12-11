using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Data.Migrations
{
    public partial class removeb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LibraryBranchId",
                table: "LibraryAssets");

            migrationBuilder.RenameColumn(
                name: "LibraryBranchId",
                table: "LibraryAssets",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_LibraryAssets_LibraryBranchId",
                table: "LibraryAssets",
                newName: "IX_LibraryAssets_LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LocationId",
                table: "LibraryAssets",
                column: "LocationId",
                principalTable: "LibraryBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LocationId",
                table: "LibraryAssets");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "LibraryAssets",
                newName: "LibraryBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_LibraryAssets_LocationId",
                table: "LibraryAssets",
                newName: "IX_LibraryAssets_LibraryBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAssets_LibraryBranches_LibraryBranchId",
                table: "LibraryAssets",
                column: "LibraryBranchId",
                principalTable: "LibraryBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
