module Shared.Tests

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open Shared
open System

let shared = testList "Shared" [
    testCase "newTransaction with invalid quantity returns Error" <| fun _ ->
        let result = ExchangeCurrency.newTransaction DateTime.Now Currency.BRL Currency.USD 220.50m 0 { Id = 0; Name = "Provider" }

        Expect.isError result "Should be error"

    testCase "newTransaction with invalid price returns Error" <| fun _ ->
        let result = ExchangeCurrency.newTransaction DateTime.Now Currency.BRL Currency.USD 0m 100 { Id = 0; Name = "Provider" }

        Expect.isError result "Should be error"
    
    testCase "newTransaction with valid parameters returns ExchangeTransaction" <| fun _ ->
        let date = DateTime.Now
        // TODO: How could I deconstruct generic Result and Value instead of specific pattern matching?
        let (Ok created) = ExchangeCurrency.newTransaction date Currency.BRL Currency.USD 220.50m 100 { Id = 0; Name = "Provider" }
        let expected = { Id = 0
                         Date = date
                         // TODO: Set up aggregation for Currency Pair in EF
                         // Pair = { FromCurrency = Currency.BRL; ToCurrency = Currency.USD }
                         FromCurrency = Currency.BRL
                         ToCurrency = Currency.USD
                         Price = 220.50m
                         Quantity = 100
                         // FIXME: Many-to-Many is poorly supported in EFCore 3.x but it'll work in 5.x
                         // Events = [{ Id = 1; Name = "China Holiday" }; { Id = 2; Name = "Brexit Annoucement" }]
                         Provider = { Id = 0; Name = "Provider" } }
        
        Expect.equal expected created "Should be equal"
]