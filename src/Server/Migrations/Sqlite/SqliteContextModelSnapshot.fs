namespace Migrations.Sqlite

open System
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Infrastructure
open Microsoft.EntityFrameworkCore.Metadata
open Microsoft.EntityFrameworkCore.Migrations
open Microsoft.EntityFrameworkCore.Storage.ValueConversion
open Repository

[<DbContext(typeof<SqliteContext>)>]
type SqliteContextModelSnapshot() =
    inherit ModelSnapshot()

    override this.BuildModel(modelBuilder: ModelBuilder) =
        modelBuilder.HasAnnotation("ProductVersion", "3.1.8")
             |> ignore

        modelBuilder.Entity("Shared.ExchangeTransaction", (fun b ->

            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER") |> ignore
            b.Property<DateTime>("Date")
                .IsRequired()
                .HasColumnType("TEXT") |> ignore
            b.Property<int>("FromCurrency")
                .IsRequired()
                .HasColumnType("INTEGER") |> ignore
            b.Property<decimal>("Price")
                .IsRequired()
                .HasColumnType("TEXT") |> ignore
            b.Property<string>("Provider")
                .HasColumnType("TEXT") |> ignore
            b.Property<int>("Quantity")
                .IsRequired()
                .HasColumnType("INTEGER") |> ignore
            b.Property<int>("ToCurrency")
                .IsRequired()
                .HasColumnType("INTEGER") |> ignore

            b.HasKey("Id") |> ignore

            b.ToTable("ExchangeTransactions") |> ignore

        )) |> ignore

