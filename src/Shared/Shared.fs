namespace Shared

open System

type Todo =
    { Id : Guid
      Description : string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITodosApi =
    { getTodos : unit -> Async<Todo list>
      addTodo : Todo -> Async<Todo> }

// ============================================================================
// Exchange Currency API
// ============================================================================

type Currency =
    | BRL = 0
    | USD = 1
    | GBP = 2

type ExchangePair =
    { FromCurrency : Currency
      ToCurrency : Currency }

type ExchangeTransaction = 
    { Id : int
      Date : DateTime
      Pair : ExchangePair
      Price : decimal
      Quantity : int
      Provider : string }
