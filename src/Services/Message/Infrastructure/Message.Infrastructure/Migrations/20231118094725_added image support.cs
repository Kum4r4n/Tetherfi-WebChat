using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Message.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedimagesupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageBytes",
                table: "Chats",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAttachement",
                table: "Chats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBytes",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "IsAttachement",
                table: "Chats");
        }
    }
}
