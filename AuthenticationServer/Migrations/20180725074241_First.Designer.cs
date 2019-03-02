﻿// <auto-generated />
using System;
using AuthenticationServer.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthenticationServer.Migrations
{
    [DbContext(typeof(AuthContext))]
    [Migration("20180725074241_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AuthenticationServer.Models.AppInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppId");

                    b.Property<string>("AppName");

                    b.Property<bool>("Deleted");

                    b.Property<int>("ManagerID");

                    b.HasKey("ID");

                    b.HasIndex("ManagerID");

                    b.ToTable("AppInfo");
                });

            modelBuilder.Entity("AuthenticationServer.Models.Manager", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email");

                    b.Property<string>("Nickname");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("Manager");
                });

            modelBuilder.Entity("AuthenticationServer.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("AuthenticationServer.Models.UserInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppInfoID");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email");

                    b.Property<string>("Nickname");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("AppInfoID");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("AuthenticationServer.Models.UserRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<int?>("RoleID");

                    b.Property<int?>("UserInfoID");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.HasIndex("UserInfoID");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("AuthenticationServer.Models.AppInfo", b =>
                {
                    b.HasOne("AuthenticationServer.Models.Manager", "Manager")
                        .WithMany("AppList")
                        .HasForeignKey("ManagerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthenticationServer.Models.UserInfo", b =>
                {
                    b.HasOne("AuthenticationServer.Models.AppInfo", "AppInfo")
                        .WithMany("UserList")
                        .HasForeignKey("AppInfoID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthenticationServer.Models.UserRole", b =>
                {
                    b.HasOne("AuthenticationServer.Models.Role", "Role")
                        .WithMany("UserRoleList")
                        .HasForeignKey("RoleID");

                    b.HasOne("AuthenticationServer.Models.UserInfo", "UserInfo")
                        .WithMany("UserRoleList")
                        .HasForeignKey("UserInfoID");
                });
#pragma warning restore 612, 618
        }
    }
}