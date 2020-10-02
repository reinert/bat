namespace Repository

open Microsoft.EntityFrameworkCore
open System.ComponentModel.DataAnnotations

open Shared

type Repository =
    abstract addTransaction : ExchangeTransaction -> Result<ExchangeTransaction, unit>
    abstract getTransactions : unit -> ExchangeTransaction list

type SqliteContext() =  
    inherit DbContext()

    [<DefaultValue>]
    val mutable exchangeTransactions : DbSet<ExchangeTransaction>
    member __.ExchangeTransactions with get() = __.exchangeTransactions
                                   and set v = __.exchangeTransactions <- v

    [<DefaultValue>]
    val mutable providers : DbSet<Provider>
    member __.Providers with get() = __.providers
                        and set v = __.providers <- v

    [<DefaultValue>]
    val mutable events : DbSet<Event>
    member __.Events with get() = __.events
                     and set v = __.events <- v

    [<DefaultValue>]
    val mutable exchangeTransactionEvents : DbSet<ExchangeTransactionEvent>
    member __.ExchangeTransactionEvents with get() = __.exchangeTransactionEvents
                                        and set v = __.exchangeTransactionEvents <- v

    override __.OnModelCreating(modelBuilder: ModelBuilder) : unit =
        modelBuilder.Entity<ExchangeTransactionEvent>()
            .HasKey([| "TransactionId"; "EventId" |]) |> ignore
        
        modelBuilder.Entity<ExchangeTransactionEvent>((fun e ->
                e.HasOne<ExchangeTransaction>("Transaction")
                    .WithMany()
                    .HasForeignKey("TransactionId") |> ignore
            )) |> ignore        

        modelBuilder.Entity<ExchangeTransactionEvent>((fun e ->
                e.HasOne<Event>("Event")
                    .WithMany()
                    .HasForeignKey("EventId") |> ignore
            )) |> ignore

        // modelBuilder.Entity("BloggingModel+Post", (fun b ->
        //         b.HasOne("BloggingModel+Blog","Blog")
        //             .WithMany()
        //             .HasForeignKey("BlogId")
        //             .OnDelete(DeleteBehavior.Cascade)
        //             .IsRequired() |> ignore
        //     )) |> ignore

    override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
        options.UseSqlite("Data Source=../../.data/sqlite/exchange_currency.db") |> ignore

type SqliteRepository() =
    interface Repository with
        member this.addTransaction (t: ExchangeTransaction) =
            // TODO: set up a connection pool
            use ctx = new SqliteContext()
            ctx.ExchangeTransactions.Add t |> ignore
            ctx.SaveChanges() |> ignore
            ctx.Dispose()
            Ok t

        member this.getTransactions () =
            // TODO: set up a connection pool
            use ctx = new SqliteContext()
            let ts = ctx.ExchangeTransactions
            ctx.Dispose()
            List.ofSeq ts
