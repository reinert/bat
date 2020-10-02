namespace Repository

open Microsoft.EntityFrameworkCore
open System.ComponentModel.DataAnnotations

open Shared

type IRepository =
    abstract addProvider : Provider -> Result<Provider, unit>
    abstract getProviders : unit -> Provider list
    
    abstract addTransaction : ExchangeTransaction -> Result<ExchangeTransaction, unit>
    abstract getTransactions : unit -> ExchangeTransaction list

type BaseContext() =  
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

type SqliteContext() =
    inherit BaseContext()

    override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
        options.UseSqlite("Data Source=../../.data/sqlite/exchange_currency.db") |> ignore

type PgsqlContext() =
    inherit BaseContext()

    override __.OnConfiguring(options: DbContextOptionsBuilder) : unit =
        options
            .UseNpgsql("Host=db;Database=bat;Username=bat;Password=bat")
            .UseSnakeCaseNamingConvention()
            |> ignore

[<AbstractClass>]
type EfRepository() =
    abstract getContext : unit -> BaseContext
    
    interface IRepository with
        member this.addProvider (e: Provider) =
            // TODO: set up a connection pool
            use ctx = this.getContext()
            ctx.Providers.Add e |> ignore
            ctx.SaveChanges() |> ignore
            // ctx.Dispose()
            Ok e
        
        member this.getProviders () =
            // TODO: set up a connection pool
            use ctx = this.getContext()
            let result = ctx.Providers
            // ctx.Dispose()
            List.ofSeq result

        member this.addTransaction (t: ExchangeTransaction) =
            // TODO: set up a connection pool
            use ctx = this.getContext()
            ctx.ExchangeTransactions.Add t |> ignore
            ctx.SaveChanges() |> ignore
            // ctx.Dispose()
            Ok t

        member this.getTransactions () =
            // TODO: set up a connection pool
            use ctx = this.getContext()
            let result = ctx.ExchangeTransactions
                            .Include("Provider")
            List.ofSeq result

type SqliteRepository() =
    inherit EfRepository()

    override __.getContext () =
        new SqliteContext() :> BaseContext

type PgsqlRepository() =
    inherit EfRepository()

    override __.getContext () =
        new PgsqlContext() :> BaseContext

type Database =
    | Sqlite
    | PostgreSql

module Repository =
    let getRepository (database: Database) =
        match database with
        | Database.Sqlite -> new SqliteRepository() :> IRepository
        | Database.PostgreSql -> new PgsqlRepository() :> IRepository
