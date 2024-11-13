using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolChat.Service.Migrations
{
    /// <inheritdoc />
    public partial class ReadMessageStatusContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadMessageStatus_Messages_MessageId",
                table: "ReadMessageStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadMessageStatus",
                table: "ReadMessageStatus");

            migrationBuilder.RenameTable(
                name: "ReadMessageStatus",
                newName: "ReadMessageStatuses");

            migrationBuilder.RenameIndex(
                name: "IX_ReadMessageStatus_MessageId",
                table: "ReadMessageStatuses",
                newName: "IX_ReadMessageStatuses_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadMessageStatuses",
                table: "ReadMessageStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadMessageStatuses_Messages_MessageId",
                table: "ReadMessageStatuses",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadMessageStatuses_Messages_MessageId",
                table: "ReadMessageStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadMessageStatuses",
                table: "ReadMessageStatuses");

            migrationBuilder.RenameTable(
                name: "ReadMessageStatuses",
                newName: "ReadMessageStatus");

            migrationBuilder.RenameIndex(
                name: "IX_ReadMessageStatuses_MessageId",
                table: "ReadMessageStatus",
                newName: "IX_ReadMessageStatus_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadMessageStatus",
                table: "ReadMessageStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadMessageStatus_Messages_MessageId",
                table: "ReadMessageStatus",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
