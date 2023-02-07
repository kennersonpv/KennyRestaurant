﻿// <auto-generated />
using Kenny.Services.ProductAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kenny.Services.ProductAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Kenny.Services.ProductAPI.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryName = "Meal",
                            Description = "Black beans with meat and rice",
                            ImageUrl = "https://assets.unileversolutions.com/recipes-v2/54349.jpg",
                            Name = "Feijoada",
                            Price = 15.0
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryName = "Sandwich",
                            Description = "It's our best seller. Picanha meat with cheddar cheese and a lot of bacon",
                            ImageUrl = "https://s2.glbimg.com/jJirZVMNK5ZsZ9UDLKQBqPu3iXk=/620x455/e.glbimg.com/og/ed/f/original/2020/10/20/hamburgueria_bob_beef_-_dia_das_criancas_-_foto_pfz_studio__norma_lima.jpg",
                            Name = "Cheesburguer",
                            Price = 29.989999999999998
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryName = "Dessert",
                            Description = "Chocolate balls with grainy chocolate",
                            ImageUrl = "https://img.itdg.com.br/tdg/images/recipes/000/000/114/201447/201447_original.jpg",
                            Name = "Brigadeiro",
                            Price = 10.99
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryName = "Meal",
                            Description = "Juicy meet with rice and fries",
                            ImageUrl = "https://marettimo.com.br/blog/wp-content/uploads/2022/10/Como-identificar-a-carne-que-parece-picanha.jpg",
                            Name = "Picanha",
                            Price = 45.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
