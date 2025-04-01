using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCatalog.Migrations
{
    /// <inheritdoc />
    public partial class joinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CatTag",
                columns: table => new
                {
                    CatsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatTag", x => new { x.CatsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CatTag_Cat_CatsId",
                        column: x => x.CatsId,
                        principalTable: "Cat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatTag_TagsId",
                table: "CatTag",
                column: "TagsId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Cat_CatId",
                table: "CatTags");

            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Tag_TagId",
                table: "CatTags");

            migrationBuilder.DropTable(
                name: "CatTag");

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
    }
}
