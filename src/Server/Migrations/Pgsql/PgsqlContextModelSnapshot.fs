namespace Migrations.Pgsql

open System
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Infrastructure
open Microsoft.EntityFrameworkCore.Metadata
open Microsoft.EntityFrameworkCore.Migrations
open Microsoft.EntityFrameworkCore.Storage.ValueConversion
open Npgsql.EntityFrameworkCore.PostgreSQL.Metadata
open Repository

[<DbContext(typeof<PgsqlContext>)>]
type PgsqlContextModelSnapshot() =
    inherit ModelSnapshot()

    override this.BuildModel(modelBuilder: ModelBuilder) =
        modelBuilder.HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
            .HasAnnotation("ProductVersion", "3.1.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
             |> ignore

        modelBuilder.Entity("Shared.Event", (fun b ->

            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnName("id")
                .HasColumnType("integer").HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                 |> ignore
            b.Property<string>("Name")
                .HasColumnName("name")
                .HasColumnType("text") |> ignore

            b.HasKey("Id")
                .HasName("pk_events") |> ignore

            b.ToTable("events") |> ignore

        )) |> ignore

        modelBuilder.Entity("Shared.ExchangeTransaction", (fun b ->

            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnName("id")
                .HasColumnType("integer").HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                 |> ignore
            b.Property<DateTime>("Date")
                .IsRequired()
                .HasColumnName("date")
                .HasColumnType("timestamp without time zone") |> ignore
            b.Property<int>("FromCurrency")
                .IsRequired()
                .HasColumnName("from_currency")
                .HasColumnType("integer") |> ignore
            b.Property<decimal>("Price")
                .IsRequired()
                .HasColumnName("price")
                .HasColumnType("numeric") |> ignore
            b.Property<Nullable<int>>("ProviderId")
                .HasColumnName("provider_id")
                .HasColumnType("integer") |> ignore
            b.Property<int>("Quantity")
                .IsRequired()
                .HasColumnName("quantity")
                .HasColumnType("integer") |> ignore
            b.Property<int>("ToCurrency")
                .IsRequired()
                .HasColumnName("to_currency")
                .HasColumnType("integer") |> ignore

            b.HasKey("Id")
                .HasName("pk_exchange_transactions") |> ignore


            b.HasIndex("ProviderId")
                .HasName("ix_exchange_transactions_provider_id") |> ignore

            b.ToTable("exchange_transactions") |> ignore

        )) |> ignore

        modelBuilder.Entity("Shared.ExchangeTransactionEvent", (fun b ->

            b.Property<int>("TransactionId")
                .HasColumnName("transaction_id")
                .HasColumnType("integer") |> ignore
            b.Property<int>("EventId")
                .HasColumnName("event_id")
                .HasColumnType("integer") |> ignore

            b.HasKey("TransactionId", "EventId")
                .HasName("pk_exchange_transaction_events") |> ignore


            b.HasIndex("EventId")
                .HasName("ix_exchange_transaction_events_event_id") |> ignore

            b.ToTable("exchange_transaction_events") |> ignore

        )) |> ignore

        modelBuilder.Entity("Shared.Provider", (fun b ->

            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnName("id")
                .HasColumnType("integer").HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                 |> ignore
            b.Property<string>("Name")
                .HasColumnName("name")
                .HasColumnType("text") |> ignore

            b.HasKey("Id")
                .HasName("pk_providers") |> ignore

            b.ToTable("providers") |> ignore

        )) |> ignore

        modelBuilder.Entity("Shared.ExchangeTransaction", (fun b ->
            b.HasOne("Shared.Provider","Provider")
                .WithMany()
                .HasForeignKey("ProviderId")
                .HasConstraintName("fk_exchange_transactions_providers_provider_id") |> ignore
        )) |> ignore

        modelBuilder.Entity("Shared.ExchangeTransactionEvent", (fun b ->
            b.HasOne("Shared.Event","Event")
                .WithMany()
                .HasForeignKey("EventId")
                .HasConstraintName("fk_exchange_transaction_events_events_event_id")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired() |> ignore
            b.HasOne("Shared.ExchangeTransaction","Transaction")
                .WithMany()
                .HasForeignKey("TransactionId")
                .HasConstraintName("fk_exchange_transaction_events_exchange_transactions_transacti")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired() |> ignore
        )) |> ignore

