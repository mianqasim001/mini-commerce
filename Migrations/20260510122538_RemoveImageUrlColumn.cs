using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageUrlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "PRODUCT");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PRODUCT",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_category_id",
                table: "PRODUCT",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCT_CATEGORY_category_id",
                table: "PRODUCT",
                column: "category_id",
                principalTable: "CATEGORY",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCT_CATEGORY_category_id",
                table: "PRODUCT");

            migrationBuilder.DropIndex(
                name: "IX_PRODUCT_category_id",
                table: "PRODUCT");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PRODUCT");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "PRODUCT",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
