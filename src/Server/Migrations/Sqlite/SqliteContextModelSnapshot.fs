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

        modelBuilder.Entity("Shared.Event", (fun b ->

            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER") |> ignore
            b.Property<string>("Name")
                .HasColumnType("TEXT") |> ignore

            b.HasKey("Id") |> ignore

            b.ToTable("Events") |> ignore

        )) |> ignore

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
            b.Property<Nullable<int>>("ProviderId")
                .HasColumnType("INTEGER") |> ignore
            b.Property<int>("Quantity")
                .IsRequired()
                .HasColumnType("INTEGER") |> ignore
            b.Property<int>("ToCurrency")
                .IsRequired()
                .HasColumnType("INTEGER") |> ignore

            b.HasKey("Id") |> ignore


            b.HasIndex("ProviderId") |> ignore

            b.ToTable("ExchangeTransactions") |> ignore

        )) |> ignore

        modelBuilder.Entity("Shared.ExchangeTransactionEvent", (fun b ->

            b.Property<int>("TransactionId")
                .HasColumnType("INTEGER") |> ignore
            b.Property<int>("EventId")
                .HasColumnType("INTEGER") |> ignore

            b.HasKey("TransactionId", "EventId") |> ignore


            b.HasIndex("EventId") |> ignore

            b.ToTable("ExchangeTransactionEvents") |> ignore

        )) |> ignore

        modelBuilder.Entity("Shared.Provider", (fun b ->

            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER") |> ignore
            b.Property<string>("Name")
                .HasColumnType("TEXT") |> ignore

            b.HasKey("Id") |> ignore

            b.ToTable("Providers") |> ignore

        )) |> ignore

        modelBuilder.Entity("Shared.ExchangeTransaction", (fun b ->
            b.HasOne("Shared.Provider","Provider")
                .WithMany()
                .HasForeignKey("ProviderId") |> ignore
        )) |> ignore

        modelBuilder.Entity("Shared.ExchangeTransactionEvent", (fun b ->
            b.HasOne("Shared.Event","Event")
                .WithMany()
                .HasForeignKey("EventId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired() |> ignore
            b.HasOne("Shared.ExchangeTransaction","Transaction")
                .WithMany()
                .HasForeignKey("TransactionId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired() |> ignore
        )) |> ignore

