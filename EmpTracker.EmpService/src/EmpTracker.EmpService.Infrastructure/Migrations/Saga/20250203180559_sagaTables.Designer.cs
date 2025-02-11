﻿// <auto-generated />
using System;
using EmpTracker.EmpService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmpTracker.EmpService.Infrastructure.Migrations.Saga
{
    [DbContext(typeof(SagaContext))]
    [Migration("20250203180559_sagaTables")]
    partial class sagaTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("saga")
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmpTracker.EmpService.Core.Domain.SagaState.EmployeeCreationState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DepartmentRetryCount")
                        .HasColumnType("int");

                    b.Property<bool>("DepartmentTotalEmployeeCountIncreased")
                        .HasColumnType("bit");

                    b.Property<Guid?>("DesignationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DesignationRetryCount")
                        .HasColumnType("int");

                    b.Property<bool>("DesignationTotalEmployeeCountIncreased")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EmployeeRetryCount")
                        .HasColumnType("int");

                    b.Property<int>("IdentityRetryCount")
                        .HasColumnType("int");

                    b.HasKey("CorrelationId");

                    b.ToTable("EmployeeCreationStates", "saga");
                });

            modelBuilder.Entity("EmpTracker.EmpService.Core.Domain.SagaState.EmployeeDeletionState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DepartmentRetryCount")
                        .HasColumnType("int");

                    b.Property<bool>("DepartmentTotalEmployeeCountDecreased")
                        .HasColumnType("bit");

                    b.Property<Guid?>("DesignationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DesignationRetryCount")
                        .HasColumnType("int");

                    b.Property<bool>("DesignationTotalEmployeeCountDecreased")
                        .HasColumnType("bit");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IdentityRetryCount")
                        .HasColumnType("int");

                    b.HasKey("CorrelationId");

                    b.ToTable("EmployeeDeletionStates", "saga");
                });
#pragma warning restore 612, 618
        }
    }
}
