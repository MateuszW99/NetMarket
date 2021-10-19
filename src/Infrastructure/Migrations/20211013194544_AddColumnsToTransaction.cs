using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddColumnsToTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Payout",
                table: "Transactions",
                newName: "TotalBuyerCost");

            migrationBuilder.AddColumn<decimal>(
                name: "CompanyProfit",
                table: "Transactions",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellerPayout",
                table: "Transactions",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Items",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyProfit",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SellerPayout",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "TotalBuyerCost",
                table: "Transactions",
                newName: "Payout");
        }
    }
}
