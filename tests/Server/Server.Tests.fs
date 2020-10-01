module Server.Tests

open Expecto

open Repository
open Shared
open System

let server = testList "Server" [
    testCase "Adding valid Transaction" <| fun _ ->
        let storage = TStorage()
        let t = { Id = 0
                  Date = DateTime.Now
                  Pair = { FromCurrency = Currency.BRL; ToCurrency = Currency.USD }
                  Price = 220.50m
                  Quantity = 100
                  Provider = "Provider" }
        let expectedResult = Ok t

        let result = storage.CreateTransaction t

        Expect.equal result expectedResult "Result should be ok"
        Expect.contains (storage.GetTransactions()) t "Storage should contain new transaction"
]

let all =
    testList "All"
        [
            Shared.Tests.shared
            server
        ]

[<EntryPoint>]
let main _ = runTests defaultConfig all