using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace books.Migrations;

/// <inheritdoc />
public partial class _20261120105112_AddPaymentFieldsToOrder : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "PaymentMethod",
            table: "Orders",
            type: "TEXT",
            nullable: false,
            defaultValue: "При получении");

        migrationBuilder.AddColumn<string>(
            name: "PaymentStatus",
            table: "Orders",
            type: "TEXT",
            nullable: false,
            defaultValue: "Оплата при получении");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PaymentMethod",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "PaymentStatus",
            table: "Orders");
    }
}
