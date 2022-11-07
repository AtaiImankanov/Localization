using Microsoft.EntityFrameworkCore.Migrations;

namespace lavAspMvclast.Migrations
{
    public partial class AddedIdForCreaterAndCompliter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompliterId",
                table: "ToDoTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreaterId",
                table: "ToDoTasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompliterId",
                table: "ToDoTasks");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "ToDoTasks");
        }
    }
}
