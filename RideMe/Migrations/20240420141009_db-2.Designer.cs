﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RideMe.Data;

#nullable disable

namespace RideMe.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240420141009_db-2")]
    partial class db2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RideMe.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("password");

                    b.HasKey("Id")
                        .HasName("PK__admin__3213E83FB474A7C9");

                    b.ToTable("admin", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__city__3213E83F7F5061D5");

                    b.ToTable("city", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Available")
                        .HasColumnType("bit")
                        .HasColumnName("available");

                    b.Property<double?>("AvgRating")
                        .HasColumnType("float")
                        .HasColumnName("avg_rating");

                    b.Property<string>("CarType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("car_type");

                    b.Property<int>("CityId")
                        .HasColumnType("int")
                        .HasColumnName("city_id");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("region");

                    b.Property<bool>("Smoking")
                        .HasColumnType("bit")
                        .HasColumnName("smoking");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK__driver__3213E83F023A7C87");

                    b.HasIndex("CityId");

                    b.HasIndex(new[] { "UserId" }, "UQ__driver__B9BE370EA02AD236")
                        .IsUnique();

                    b.ToTable("driver", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.Passenger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK__passenge__3213E83F0359D320");

                    b.HasIndex(new[] { "UserId" }, "UQ__passenge__B9BE370E17792936")
                        .IsUnique();

                    b.ToTable("passenger", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.Ride", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DriverId")
                        .HasColumnType("int")
                        .HasColumnName("driver_id");

                    b.Property<string>("Feedback")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("feedback");

                    b.Property<int>("PassengerId")
                        .HasColumnType("int")
                        .HasColumnName("passenger_id");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("price");

                    b.Property<int?>("Rating")
                        .HasColumnType("int")
                        .HasColumnName("rating");

                    b.Property<DateTime>("RideDate")
                        .HasColumnType("datetime")
                        .HasColumnName("ride_date");

                    b.Property<string>("RideDestination")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ride_destination");

                    b.Property<string>("RideSource")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ride_source");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("status_id");

                    b.HasKey("Id")
                        .HasName("PK__ride__3213E83F19DFBE03");

                    b.HasIndex("DriverId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("StatusId");

                    b.ToTable("ride", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.RideStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__ride_sta__3213E83F64B4F2F6");

                    b.ToTable("ride_status", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__role__3213E83F74C1F6E7");

                    b.ToTable("role", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("phone_number");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("status_id");

                    b.HasKey("Id")
                        .HasName("PK__user__3213E83F77213F8A");

                    b.HasIndex("RoleId");

                    b.HasIndex("StatusId");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.UserStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK__user_sta__3213E83F275D53B7");

                    b.ToTable("user_status", (string)null);
                });

            modelBuilder.Entity("RideMe.Models.Driver", b =>
                {
                    b.HasOne("RideMe.Models.City", "City")
                        .WithMany("Drivers")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__driver__city_id__46E78A0C");

                    b.HasOne("RideMe.Models.User", "User")
                        .WithOne("Driver")
                        .HasForeignKey("RideMe.Models.Driver", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__driver__user_id__45F365D3");

                    b.Navigation("City");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RideMe.Models.Passenger", b =>
                {
                    b.HasOne("RideMe.Models.User", "User")
                        .WithOne("Passenger")
                        .HasForeignKey("RideMe.Models.Passenger", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__passenger__user___4AB81AF0");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RideMe.Models.Ride", b =>
                {
                    b.HasOne("RideMe.Models.Driver", "Driver")
                        .WithMany("Rides")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ride__driver_id__4E88ABD4");

                    b.HasOne("RideMe.Models.Passenger", "Passenger")
                        .WithMany("Rides")
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ride__passenger___4D94879B");

                    b.HasOne("RideMe.Models.RideStatus", "Status")
                        .WithMany("Rides")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ride__status_id__4F7CD00D");

                    b.Navigation("Driver");

                    b.Navigation("Passenger");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("RideMe.Models.User", b =>
                {
                    b.HasOne("RideMe.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__user__role_id__412EB0B6");

                    b.HasOne("RideMe.Models.UserStatus", "Status")
                        .WithMany("Users")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__user__status_id__4222D4EF");

                    b.Navigation("Role");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("RideMe.Models.City", b =>
                {
                    b.Navigation("Drivers");
                });

            modelBuilder.Entity("RideMe.Models.Driver", b =>
                {
                    b.Navigation("Rides");
                });

            modelBuilder.Entity("RideMe.Models.Passenger", b =>
                {
                    b.Navigation("Rides");
                });

            modelBuilder.Entity("RideMe.Models.RideStatus", b =>
                {
                    b.Navigation("Rides");
                });

            modelBuilder.Entity("RideMe.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("RideMe.Models.User", b =>
                {
                    b.Navigation("Driver");

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("RideMe.Models.UserStatus", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
