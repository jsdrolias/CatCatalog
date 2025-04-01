using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatCatalog.Migrations
{
    /// <inheritdoc />
    public partial class joinTableusingsimple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatTags");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatTag");

            migrationBuilder.CreateTable(
                name: "CatTags",
                columns: table => new
                {
                    CatId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatTags", x => new { x.CatId, x.TagId });
                    table.ForeignKey(
                        name: "FK_CatTags_Cat_CatId",
                        column: x => x.CatId,
                        principalTable: "Cat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatTags_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatTags_TagId",
                table: "CatTags",
                column: "TagId");
        }
    }
}
