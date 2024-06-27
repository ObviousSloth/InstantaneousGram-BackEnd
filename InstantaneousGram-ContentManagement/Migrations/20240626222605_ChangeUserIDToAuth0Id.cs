using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstantaneousGram_ContentManagement.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIDToAuth0Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ContentManagements",
                newName: "Auth0Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Auth0Id",
                table: "ContentManagements",
                newName: "UserID");
        }
    }
}
