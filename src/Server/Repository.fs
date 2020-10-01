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
    val mutable m_exchange_transactions : DbSet<ExchangeTransaction>
    member __.ExchangeTransactions with get() = __.m_exchange_transactions
                                   and set v = __.m_exchange_transactions <- v

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
