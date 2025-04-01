using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCatalog.Migrations
{
    /// <inheritdoc />
    public partial class removeCatTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Cat_CatId",
                table: "CatTags");

            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Tag_TagId",
                table: "CatTags");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "CatTags",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "CatId",
                table: "CatTags",
                newName: "CatsId");

            migrationBuilder.RenameIndex(
                name: "IX_CatTags_TagId",
                table: "CatTags",
                newName: "IX_CatTags_TagsId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Job",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CatId",
                table: "Cat",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CatTags_Cat_CatsId",
                table: "CatTags",
                column: "CatsId",
                principalTable: "Cat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatTags_Tag_TagsId",
                table: "CatTags",
                column: "TagsId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Cat_CatsId",
                table: "CatTags");

            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Tag_TagsId",
                table: "CatTags");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "CatTags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "CatsId",
                table: "CatTags",
                newName: "CatId");

            migrationBuilder.RenameIndex(
                name: "IX_CatTags_TagsId",
                table: "CatTags",
                newName: "IX_CatTags_TagId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Job",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "CatId",
                table: "Cat",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddForeignKey(
                name: "FK_CatTags_Cat_CatId",
                table: "CatTags",
                column: "CatId",
                principalTable: "Cat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatTags_Tag_TagId",
                table: "CatTags",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
