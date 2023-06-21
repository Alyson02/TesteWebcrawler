﻿// <auto-generated />
using System;
using Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(TesteElawDbContext))]
    partial class TesteElawDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Domain.Entities.ExecutionEntity", b =>
                {
                    b.Property<int>("ExecutionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ExecutionId"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("JsonFile")
                        .HasColumnType("text");

                    b.Property<int?>("LineNumbers")
                        .HasColumnType("integer");

                    b.Property<int>("PageNumbers")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ExecutionId");

                    b.ToTable("Execution");
                });
#pragma warning restore 612, 618
        }
    }
}