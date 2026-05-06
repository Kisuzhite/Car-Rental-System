using Car_Rental_System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Rental_System.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260506192000_AddCustomerArchiveAndDuplicateIndexes")]
    public partial class AddCustomerArchiveAndDuplicateIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_PlateNumber",
                table: "Cars",
                column: "PlateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LicenseNumber",
                table: "Customers",
                column: "LicenseNumber",
                unique: true,
                filter: "[LicenseNumber] <> N'Not provided'");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Cars_PlateNumber",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LicenseNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Customers");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
