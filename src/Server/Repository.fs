namespace Repository

open Shared

type TStorage () =
    let transactions = ResizeArray<_>()

    member __.GetTransactions () =
        List.ofSeq transactions

    member __.CreateTransaction (t: ExchangeTransaction) =
        let result = ExchangeCurrency.validateTransaction t.Price t.Quantity
        match result with
        | Error e -> Error e
        | Ok _ ->
            transactions.Add t
            Ok t
