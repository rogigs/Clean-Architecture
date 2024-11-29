using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean_Architecture.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributeUsersToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserIds",
                table: "Projects",
                newName: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Projects",
                newName: "UserIds");
        }
    }
}
