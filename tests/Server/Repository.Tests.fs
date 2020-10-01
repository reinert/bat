module Repository.Tests

open Expecto

open Repository
open Shared
open System

let repository = testList "Repository" [
    testCase "addTransaction with a valid Transaction should save it in the DB and return ok" <| fun _ ->
        // TODO: How could I deconstruct generic Result and Value instead of specific pattern matching?
        let (Ok t) = ExchangeCurrency.newTransaction DateTime.Now Currency.BRL Currency.USD 220.50m 100 "Provider"
        let expectedResult = Ok t

        let result = Repository.addTransaction t

        Expect.equal result expectedResult "Result should be ok"
        Expect.contains (Repository.getTransactions()) t "Storage should contain new transaction"
]
