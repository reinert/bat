namespace Shared

open Microsoft.EntityFrameworkCore
open System.ComponentModel.DataAnnotations
open System

type Currency =
    | BRL = 0
    | USD = 1
    | GBP = 2

// TODO: Set up aggregation for ExchangePair in EF
// type ExchangePair =
//     { FromCurrency : Currency
//       ToCurrency : Currency }

[<CLIMutable>]
type ExchangeTransaction = 
    { [<Key>] Id : int
      Date : DateTime
      // TODO: Set up aggregation for ExchangePair in EF
      // Pair : ExchangePair
      FromCurrency : Currency
      ToCurrency : Currency
      Price : decimal
      Quantity : int
      Provider : string }

module ExchangeCurrency =
    let validateTransaction (price: decimal) (quantity: int) =
        if price <= 0m then Error "price cannot be less or equal to zero"
        else if quantity <= 0 then Error "quantity cannot be less or equal to zero"
        else Ok ()

    let newTransaction (date: DateTime) (fromCurrency: Currency) (toCurrency: Currency) (price: decimal) (quantity: int) (provider: string) =
        validateTransaction price quantity |> function
        | Error e -> Error e
        | Ok _ -> Ok { Id = 0
                       Date = date
                       // TODO: Set up aggregation for ExchangePair in EF
                       // Pair = { FromCurrency = fromCurrency; ToCurrency = toCurrency }
                       FromCurrency = fromCurrency
                       ToCurrency = toCurrency
                       Price = price
                       Quantity = quantity
                       Provider = provider }
