namespace Shared

open System.ComponentModel.DataAnnotations
open System

[<CLIMutable>]
type Event =
    { [<Key>] Id: int
      Name: string }

type Currency =
    | BRL = 0
    | USD = 1
    | GBP = 2

// TODO: Set up aggregation for ExchangePair in EF
// type ExchangePair =
//     { FromCurrency: Currency
//       ToCurrency: Currency }

[<CLIMutable>]
type Provider =
    { [<Key>] Id: int
      Name: string }

[<CLIMutable>]
type ExchangeTransaction = 
    { [<Key>] Id: int
      Date : DateTime
      // TODO: Set up aggregation for ExchangePair in EF
      // Pair : ExchangePair
      FromCurrency: Currency
      ToCurrency: Currency
      Price: decimal
      Quantity: int
      // FIXME: Many-to-Many is poorly supported in EFCore 3.x but it'll work in 5.x
      // Events: List<Event>
      Provider: Provider }

[<CLIMutable>]
type ExchangeTransactionEvent =
    { TransactionId: int
      EventId: int
      Transaction: ExchangeTransaction
      Event: Event }
    
module ExchangeCurrency =
    let validateTransaction (price: decimal) (quantity: int) =
        if price <= 0m then Error "price cannot be less or equal to zero"
        else if quantity <= 0 then Error "quantity cannot be less or equal to zero"
        else Ok ()

    let newTransaction (date: DateTime) (fromCurrency: Currency) (toCurrency: Currency) (price: decimal) (quantity: int) (provider: Provider) =
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
                       // FIXME: Many-to-Many is poorly supported in EFCore 3.x but it'll work in 5.x
                       // Events = [{ Id = 1; Name = "China Holiday" }; { Id = 2; Name = "Brexit Annoucement" }]
                       Provider = provider }
       
    let newProvider (name: string) =
        Ok { Id = 0; Name = name }