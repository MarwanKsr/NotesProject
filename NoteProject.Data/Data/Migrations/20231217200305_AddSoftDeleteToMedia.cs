using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteProject.Data.data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Medias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Medias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medias",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medias");
        }
    }
}
