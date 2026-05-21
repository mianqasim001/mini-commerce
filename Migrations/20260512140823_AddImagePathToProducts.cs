using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "PRODUCT",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CART_ITEM_cart_id",
                table: "CART_ITEM",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_CART_ITEM_product_id",
                table: "CART_ITEM",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_CART_ITEM_CART_cart_id",
                table: "CART_ITEM",
                column: "cart_id",
                principalTable: "CART",
                principalColumn: "cart_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CART_ITEM_PRODUCT_product_id",
                table: "CART_ITEM",
                column: "product_id",
                principalTable: "PRODUCT",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CART_ITEM_CART_cart_id",
                table: "CART_ITEM");

            migrationBuilder.DropForeignKey(
                name: "FK_CART_ITEM_PRODUCT_product_id",
                table: "CART_ITEM");

            migrationBuilder.DropIndex(
                name: "IX_CART_ITEM_cart_id",
                table: "CART_ITEM");

            migrationBuilder.DropIndex(
                name: "IX_CART_ITEM_product_id",
                table: "CART_ITEM");

            migrationBuilder.DropColumn(
                name: "image_path",
                table: "PRODUCT");
        }
    }
}
