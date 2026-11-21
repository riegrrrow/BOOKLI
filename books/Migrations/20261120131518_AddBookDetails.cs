using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace books.Migrations;

/// <inheritdoc />
public partial class _20261120131518_AddBookDetails : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CoverType",
            table: "Books",
            type: "TEXT",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<int>(
            name: "PageCount",
            table: "Books",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "PublicationYear",
            table: "Books",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<decimal>(
            name: "Rating",
            table: "Books",
            type: "TEXT",
            nullable: false,
            defaultValue: 0m);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CoverType",
            table: "Books");

        migrationBuilder.DropColumn(
            name: "PageCount",
            table: "Books");

        migrationBuilder.DropColumn(
            name: "PublicationYear",
            table: "Books");

        migrationBuilder.DropColumn(
            name: "Rating",
            table: "Books");
    }
}
