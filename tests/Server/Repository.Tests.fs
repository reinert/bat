module Repository.Tests

open Expecto

open Microsoft.EntityFrameworkCore
open Repository
open Shared
open System


let clearTable (repository: IRepository) (table: string) =
    let ctx = (repository :?> EfRepository).getContext()
    (ctx :> DbContext).Database.ExecuteSqlRaw("DELETE from " + table)

//=============================================================================
// Provider
//=============================================================================

let addProviderValid (repository: IRepository) =
    // TODO: How could I deconstruct generic Result and Value instead of specific pattern matching?
    let (Ok p) = ExchangeCurrency.newProvider "Provider"
    let expectedResult = Ok p

    printfn "Added %A" p

    let result = repository.addProvider p

    printfn "Saved %A" <| repository.getProviders()

    Expect.equal result expectedResult "Result should be ok"


//=============================================================================
// ExchangeTransaction
//=============================================================================

let addTransactionValid (repository: IRepository) =
    // TODO: How could I deconstruct generic Result and Value instead of specific pattern matching?
    let (Ok p) = ExchangeCurrency.newProvider "Provider"
    let (Ok t) = ExchangeCurrency.newTransaction DateTime.Now Currency.BRL Currency.USD 220.50m 100 p
    let expectedResult = Ok t

    printfn "Added %A" t
        
    let result = repository.addTransaction t
    
    printfn "Saved %A" <| repository.getTransactions()

    Expect.equal result expectedResult "Result should be ok"


//=============================================================================
// Sqlite Tests
//=============================================================================

let sqliteRepository = testList "SqliteRepository" [
    let sqlite = Repository.getRepository Database.Sqlite

    testCase "addProvider with a valid instance should save it in the DB and return ok" <| fun _ ->
        clearTable sqlite "Providers"
        addProviderValid sqlite

    testCase "addTransaction with a valid instance should save it in the DB and return ok" <| fun _ ->
        clearTable sqlite "ExchangeTransactions"
        clearTable sqlite "Providers"
        addTransactionValid sqlite
]

//=============================================================================
// Pgsql Tests
//=============================================================================

let pgsqlRepository = testList "PgsqlRepository" [
    let pgsql = Repository.getRepository Database.PostgreSql

    testCase "addProvider with a valid instance should save it in the DB and return ok" <| fun _ ->
        clearTable pgsql "Providers"
        addProviderValid pgsql

    testCase "addTransaction with a valid instance should save it in the DB and return ok" <| fun _ ->
        clearTable pgsql "exchange_transactions"
        clearTable pgsql "providers"
        addTransactionValid pgsql
]
