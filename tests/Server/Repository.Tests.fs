module Repository.Tests

open Expecto

open Repository
open Shared
open System

let sqliteRepository = testList "SqliteRepository" [
    testCase "addTransaction with a valid Transaction should save it in the DB and return ok" <| fun _ ->
        // TODO: How could I deconstruct generic Result and Value instead of specific pattern matching?
        let (Ok t) = ExchangeCurrency.newTransaction DateTime.Now Currency.BRL Currency.USD 220.50m 100 { Id = 0; Name = "Provider" }
        let expectedResult = Ok t

        let repo = Repository.getRepository Database.Sqlite
        let result = repo.addTransaction t

        Expect.equal result expectedResult "Result should be ok"
        // Expect.contains (Repository.getTransactions()) t "Storage should contain new transaction"
]

let pgsqlRepository = testList "PgsqlRepository" [
    testCase "addTransaction with a valid Transaction should save it in the DB and return ok" <| fun _ ->
        // TODO: How could I deconstruct generic Result and Value instead of specific pattern matching?
        let (Ok t) = ExchangeCurrency.newTransaction DateTime.Now Currency.BRL Currency.USD 220.50m 100 { Id = 0; Name = "Provider" }
        let expectedResult = Ok t

        let repo = Repository.getRepository Database.PostgreSql
        let result = repo.addTransaction t

        Expect.equal result expectedResult "Result should be ok"
        // Expect.contains (Repository.getTransactions()) t "Storage should contain new transaction"
]
