using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToRoomType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RoomTypes",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RoomTypes");
        }
    }
}
