module Server.Tests

open Expecto

open Shared

let server = testList "Server" [
    testCase "Ok is Ok" <| fun _ ->
        let expected = Ok ()
        Expect.isOk expected "Result should be ok"
]

let all =
    testList "All"
        [
            Shared.Tests.shared
            Repository.Tests.sqliteRepository
            server
        ]

[<EntryPoint>]
let main _ = runTests defaultConfig all