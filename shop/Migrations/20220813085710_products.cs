using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop.Migrations
{
    public partial class products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    p_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    p_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p_brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    p_size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p_price = table.Column<int>(type: "int", nullable: false),
                    p_quantity = table.Column<int>(type: "int", nullable: false),
                    p_image_url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.p_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
