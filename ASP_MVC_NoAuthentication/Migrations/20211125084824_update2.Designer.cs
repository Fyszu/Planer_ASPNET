﻿// <auto-generated />
using System;
using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASP_MVC_NoAuthentication.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20211125084824_update2")]
    partial class update2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("ASP_MVC_NoAuthentication.Data.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Connectors")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaximumDistance")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("ASP_MVC_NoAuthentication.Data.ChargingPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Power")
                        .HasColumnType("int");

                    b.Property<int?>("StationId")
                        .HasColumnType("int");

                    b.Property<string>("TypeOfConnector")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TypeOfCurrent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("ChargingPoints");
                });

            modelBuilder.Entity("ASP_MVC_NoAuthentication.Data.ChargingStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OpenHours")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalAdress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("StationState")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ChargingStations");
                });

            modelBuilder.Entity("ASP_MVC_NoAuthentication.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DrivingStyle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HighwaySpeed")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("SummerFactor")
                        .HasColumnType("double");

                    b.Property<double>("WinterFactor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ASP_MVC_NoAuthentication.Data.ChargingPoint", b =>
                {
                    b.HasOne("ASP_MVC_NoAuthentication.Data.ChargingStation", "Station")
                        .WithMany()
                        .HasForeignKey("StationId");

                    b.Navigation("Station");
                });
#pragma warning restore 612, 618
        }
    }
}
