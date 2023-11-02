using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class GroupsAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cconnections_Groups_GroupName",
                table: "Cconnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cconnections",
                table: "Cconnections");

            migrationBuilder.RenameTable(
                name: "Cconnections",
                newName: "Connections");

            migrationBuilder.RenameIndex(
                name: "IX_Cconnections_GroupName",
                table: "Connections",
                newName: "IX_Connections_GroupName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "ConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Groups_GroupName",
                table: "Connections",
                column: "GroupName",
                principalTable: "Groups",
                principalColumn: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Groups_GroupName",
                table: "Connections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.RenameTable(
                name: "Connections",
                newName: "Cconnections");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_GroupName",
                table: "Cconnections",
                newName: "IX_Cconnections_GroupName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cconnections",
                table: "Cconnections",
                column: "ConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cconnections_Groups_GroupName",
                table: "Cconnections",
                column: "GroupName",
                principalTable: "Groups",
                principalColumn: "Name");
        }
    }
}
