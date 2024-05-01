using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideMe.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:RideMe/Migrations/20240420161443_test2.cs
    public partial class test2 : Migration
========
    public partial class db : Migration
>>>>>>>> Test:RideMe/Migrations/20240420165826_db.cs
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
                    table.PrimaryKey("PK__admin__3213E83FADF3CB5A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__city__3213E83FCAA78972", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ride_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ride_sta__3213E83FC44D3000", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__role__3213E83F41503532", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_sta__3213E83FED22055D", x => x.id);
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
                    role_id = table.Column<int>(type: "int", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user__3213E83FBE7EA2CB", x => x.id);
                    table.ForeignKey(
                        name: "FK__user__role_id__412EB0B6",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__user__status_id__4222D4EF",
                        column: x => x.status_id,
                        principalTable: "user_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "driver",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
<<<<<<<< HEAD:RideMe/Migrations/20240420161443_test2.cs
                    user_id = table.Column<int>(type: "int", nullable: true),
                    car_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    smoking = table.Column<bool>(type: "bit", nullable: true),
                    city_id = table.Column<int>(type: "int", nullable: true),
                    region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    available = table.Column<bool>(type: "bit", nullable: true),
                    avg_rating = table.Column<double>(type: "float", nullable: true)
========
                    user_id = table.Column<int>(type: "int", nullable: false),
                    car_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    smoking = table.Column<bool>(type: "bit", nullable: false),
                    city_id = table.Column<int>(type: "int", nullable: false),
                    region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    available = table.Column<bool>(type: "bit", nullable: false),
                    avg_rating = table.Column<double>(type: "float", nullable: false)
>>>>>>>> Test:RideMe/Migrations/20240420165826_db.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__driver__3213E83F157FDE69", x => x.id);
                    table.ForeignKey(
                        name: "FK__driver__city_id__46E78A0C",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id");
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
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__passenge__3213E83F02977409", x => x.id);
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
<<<<<<<< HEAD:RideMe/Migrations/20240420161443_test2.cs
                    passenger_id = table.Column<int>(type: "int", nullable: true),
                    driver_id = table.Column<int>(type: "int", nullable: true),
                    ride_source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ride_destination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    feedback = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ride_date = table.Column<DateTime>(type: "datetime", nullable: true)
========
                    passenger_id = table.Column<int>(type: "int", nullable: false),
                    driver_id = table.Column<int>(type: "int", nullable: false),
                    ride_source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ride_destination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    feedback = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ride_date = table.Column<DateTime>(type: "datetime", nullable: false)
>>>>>>>> Test:RideMe/Migrations/20240420165826_db.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ride__3213E83F43A96274", x => x.id);
                    table.ForeignKey(
                        name: "FK__ride__driver_id__4E88ABD4",
                        column: x => x.driver_id,
                        principalTable: "driver",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__ride__passenger___4D94879B",
                        column: x => x.passenger_id,
                        principalTable: "passenger",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__ride__status_id__4F7CD00D",
                        column: x => x.status_id,
                        principalTable: "ride_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_driver_city_id",
                table: "driver",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "UQ__driver__B9BE370ED55FCF0C",
                table: "driver",
                column: "user_id",
                unique: true,
                filter: "[user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__passenge__B9BE370E3F7B5D43",
                table: "passenger",
                column: "user_id",
                unique: true,
                filter: "[user_id] IS NOT NULL");

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
