using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Rental_System.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalPaymentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentalPayments",
                columns: table => new
                {
                    RentalPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalId = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PaymentReferenceNumber = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPayments", x => x.RentalPaymentId);
                    table.ForeignKey(
                        name: "FK_RentalPayments_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "RentalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("""
                INSERT INTO RentalPayments (RentalId, PaymentDate, Amount, PaymentMethod, PaymentReferenceNumber, PaymentType)
                SELECT RentalId, RentalDate, AmountPaid, PaymentMethod, N'Not provided', N'Initial'
                FROM Rentals
                WHERE AmountPaid > 0
                """);

            migrationBuilder.CreateIndex(
                name: "IX_RentalPayments_RentalId",
                table: "RentalPayments",
                column: "RentalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalPayments");
        }
    }
}
