using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEM_order_id",
                table: "ORDER_ITEM",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEM_product_id",
                table: "ORDER_ITEM",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ORDER_ITEM_ORDERS_order_id",
                table: "ORDER_ITEM",
                column: "order_id",
                principalTable: "ORDERS",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ORDER_ITEM_PRODUCT_product_id",
                table: "ORDER_ITEM",
                column: "product_id",
                principalTable: "PRODUCT",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ORDER_ITEM_ORDERS_order_id",
                table: "ORDER_ITEM");

            migrationBuilder.DropForeignKey(
                name: "FK_ORDER_ITEM_PRODUCT_product_id",
                table: "ORDER_ITEM");

            migrationBuilder.DropIndex(
                name: "IX_ORDER_ITEM_order_id",
                table: "ORDER_ITEM");

            migrationBuilder.DropIndex(
                name: "IX_ORDER_ITEM_product_id",
                table: "ORDER_ITEM");
        }
    }
}
