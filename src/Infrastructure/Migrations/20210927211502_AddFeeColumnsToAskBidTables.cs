﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddFeeColumnsToAskBidTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asks_ItemSizes_ItemSizeId",
                table: "Asks");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_ItemSizes_ItemSizeId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSizes_Items_ItemId",
                table: "ItemSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSizes_Sizes_SizeId",
                table: "ItemSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemSizes",
                table: "ItemSizes");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Asks");

            migrationBuilder.RenameTable(
                name: "ItemSizes",
                newName: "ItemSize");

            migrationBuilder.RenameColumn(
                name: "ItemSizeId",
                table: "Bids",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_ItemSizeId",
                table: "Bids",
                newName: "IX_Bids_SizeId");

            migrationBuilder.RenameColumn(
                name: "ItemSizeId",
                table: "Asks",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Asks_ItemSizeId",
                table: "Asks",
                newName: "IX_Asks_SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSizes_SizeId",
                table: "ItemSize",
                newName: "IX_ItemSize_SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSizes_ItemId",
                table: "ItemSize",
                newName: "IX_ItemSize_ItemId");

            migrationBuilder.AddColumn<decimal>(
                name: "BuyerFee",
                table: "Bids",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Bids",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Asks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "SellerFee",
                table: "Asks",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemSize",
                table: "ItemSize",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ItemId",
                table: "Bids",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Asks_ItemId",
                table: "Asks",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asks_Items_ItemId",
                table: "Asks",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asks_Sizes_SizeId",
                table: "Asks",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Items_ItemId",
                table: "Bids",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Sizes_SizeId",
                table: "Bids",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSize_Items_ItemId",
                table: "ItemSize",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSize_Sizes_SizeId",
                table: "ItemSize",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asks_Items_ItemId",
                table: "Asks");

            migrationBuilder.DropForeignKey(
                name: "FK_Asks_Sizes_SizeId",
                table: "Asks");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Items_ItemId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Sizes_SizeId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSize_Items_ItemId",
                table: "ItemSize");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSize_Sizes_SizeId",
                table: "ItemSize");

            migrationBuilder.DropIndex(
                name: "IX_Bids_ItemId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Asks_ItemId",
                table: "Asks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemSize",
                table: "ItemSize");

            migrationBuilder.DropColumn(
                name: "BuyerFee",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Asks");

            migrationBuilder.DropColumn(
                name: "SellerFee",
                table: "Asks");

            migrationBuilder.RenameTable(
                name: "ItemSize",
                newName: "ItemSizes");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "Bids",
                newName: "ItemSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_SizeId",
                table: "Bids",
                newName: "IX_Bids_ItemSizeId");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "Asks",
                newName: "ItemSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Asks_SizeId",
                table: "Asks",
                newName: "IX_Asks_ItemSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSize_SizeId",
                table: "ItemSizes",
                newName: "IX_ItemSizes_SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSize_ItemId",
                table: "ItemSizes",
                newName: "IX_ItemSizes_ItemId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Bids",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Asks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemSizes",
                table: "ItemSizes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Asks_ItemSizes_ItemSizeId",
                table: "Asks",
                column: "ItemSizeId",
                principalTable: "ItemSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_ItemSizes_ItemSizeId",
                table: "Bids",
                column: "ItemSizeId",
                principalTable: "ItemSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSizes_Items_ItemId",
                table: "ItemSizes",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSizes_Sizes_SizeId",
                table: "ItemSizes",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
