﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Users.Infrastructure.Data;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241205012855_CreateOutboxTable")]
    partial class CreateOutboxTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Users.Domain.Entities.OutboxMessage", b =>
                {
                    b.Property<Guid>("OutboxMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Processed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("OutboxMessageId");

                    b.HasIndex("OutboxMessageId");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("Users.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserId");

                    b.HasIndex("UserId", "Email");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}