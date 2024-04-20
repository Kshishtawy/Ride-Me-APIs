using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideMe.Migrations
{
    /// <inheritdoc />
    public partial class db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__admin__3213E83FB474A7C9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__city__3213E83F7F5061D5", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ride_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ride_sta__3213E83F64B4F2F6", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__role__3213E83F74C1F6E7", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_sta__3213E83F275D53B7", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user__3213E83F77213F8A", x => x.id);
                    table.ForeignKey(
                        name: "FK__user__role_id__412EB0B6",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__user__status_id__4222D4EF",
                        column: x => x.status_id,
                        principalTable: "user_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "driver",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    car_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    smoking = table.Column<bool>(type: "bit", nullable: false),
                    city_id = table.Column<int>(type: "int", nullable: false),
                    region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    available = table.Column<bool>(type: "bit", nullable: false),
                    avg_rating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__driver__3213E83F023A7C87", x => x.id);
                    table.ForeignKey(
                        name: "FK__driver__city_id__46E78A0C",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__driver__user_id__45F365D3",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "passenger",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__passenge__3213E83F0359D320", x => x.id);
                    table.ForeignKey(
                        name: "FK__passenger__user___4AB81AF0",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ride",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    passenger_id = table.Column<int>(type: "int", nullable: false),
                    driver_id = table.Column<int>(type: "int", nullable: false),
                    ride_source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ride_destination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    feedback = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ride_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ride__3213E83F19DFBE03", x => x.id);
                    table.ForeignKey(
                        name: "FK__ride__driver_id__4E88ABD4",
                        column: x => x.driver_id,
                        principalTable: "driver",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ride__passenger___4D94879B",
                        column: x => x.passenger_id,
                        principalTable: "passenger",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ride__status_id__4F7CD00D",
                        column: x => x.status_id,
                        principalTable: "ride_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_driver_city_id",
                table: "driver",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "UQ__driver__B9BE370EA02AD236",
                table: "driver",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__passenge__B9BE370E17792936",
                table: "passenger",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ride_driver_id",
                table: "ride",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_ride_passenger_id",
                table: "ride",
                column: "passenger_id");

            migrationBuilder.CreateIndex(
                name: "IX_ride_status_id",
                table: "ride",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_status_id",
                table: "user",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "ride");

            migrationBuilder.DropTable(
                name: "driver");

            migrationBuilder.DropTable(
                name: "passenger");

            migrationBuilder.DropTable(
                name: "ride_status");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "user_status");
        }
    }
}
