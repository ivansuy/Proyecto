using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class elimarbodega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bodega",
                table: "existencia");

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "existencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Idbodega",
                table: "existencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_existencia_Idbodega",
                table: "existencia",
                column: "Idbodega");

            migrationBuilder.AddForeignKey(
                name: "FK_existencia_almacen_Idbodega",
                table: "existencia",
                column: "Idbodega",
                principalTable: "almacen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_existencia_almacen_Idbodega",
                table: "existencia");

            migrationBuilder.DropIndex(
                name: "IX_existencia_Idbodega",
                table: "existencia");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "existencia");

            migrationBuilder.DropColumn(
                name: "Idbodega",
                table: "existencia");

            migrationBuilder.AddColumn<string>(
                name: "bodega",
                table: "existencia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
