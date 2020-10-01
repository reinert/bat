namespace Repository

open Microsoft.EntityFrameworkCore
open System.ComponentModel.DataAnnotations

open Shared

type SqliteContext() =  
    inherit DbContext()

    [<DefaultValue>]
    val mutable m_exchange_transactions : DbSet<ExchangeTransaction>
    member __.ExchangeTransactions with get() = __.m_exchange_transactions
                                   and set v = __.m_exchange_transactions <- v

    override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
        options.UseSqlite("Data Source=../../.data/sqlite/exchange_currency.db") |> ignore

module Repository =
    let addTransaction (t: ExchangeTransaction) =
        // TODO: set up a connection pool
        use ctx = new SqliteContext()
        ctx.ExchangeTransactions.Add t |> ignore
        ctx.SaveChanges() |> ignore
        ctx.Dispose()
        Ok t

    let getTransactions =
        // TODO: set up a connection pool
        use ctx = new SqliteContext()
        let ts = ctx.ExchangeTransactions
        ctx.Dispose()
        List.ofSeq ts
