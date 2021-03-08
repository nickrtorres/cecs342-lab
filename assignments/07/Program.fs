open ListOps

exception CheckFailedException of string

let makeCustomerWithBalance name (amount: int) =
    let account =
        if amount > 0 then
            Balance amount
        elif amount < 0 then
            Overdrawn(abs amount)
        else
            Empty

    { Name = name; Account = account }

let check actual expected =
    if actual <> expected then
        raise (CheckFailedException(sprintf "Check failed. Expected: '%A' got: '%A'." expected actual))
    else
        ()

[<EntryPoint>]
let main _ =
    try
        check (join "" []) ""
        check (join " " [ "this"; "is"; "a"; "sentence." ]) "this is a sentence."
        check (join "," [ "foo"; "bar"; "baz"; "qux" ]) "foo,bar,baz,qux"

        let bank =
            [ makeCustomerWithBalance "Jane" 100
              makeCustomerWithBalance "Joe" -200
              makeCustomerWithBalance "Jane" -100
              makeCustomerWithBalance "Joe" 0
              makeCustomerWithBalance "Jane" 100
              makeCustomerWithBalance "Joe" 50 ]

        let names = [ "Jane"; "Joe" ]

        check
            (simplifyBank bank names)
            [ makeCustomerWithBalance "Jane" 100
              makeCustomerWithBalance "Joe" -150 ]

        let names = [ "Joe"; "Bill"; "Jane" ]

        let bank =
            [ makeCustomerWithBalance "Joe" 100
              makeCustomerWithBalance "Jane" 2000
              makeCustomerWithBalance "Bill" -150
              makeCustomerWithBalance "Joe" 0
              makeCustomerWithBalance "Joe" -250
              makeCustomerWithBalance "Bill" -550
              makeCustomerWithBalance "Joe" -75
              makeCustomerWithBalance "Jane" 3000
              makeCustomerWithBalance "Joe" -100
              makeCustomerWithBalance "Bill" 150
              makeCustomerWithBalance "Joe" 150
              makeCustomerWithBalance "Jane" 4000
              makeCustomerWithBalance "Joe" -25
              makeCustomerWithBalance "Bill" 550
              makeCustomerWithBalance "Joe" -50
              makeCustomerWithBalance "Bill" 250
              makeCustomerWithBalance "Joe" 42
              makeCustomerWithBalance "Joe" -250
              makeCustomerWithBalance "Jane" 1000
              makeCustomerWithBalance "Bill" -250
              makeCustomerWithBalance "Joe" 500 ]

        check
            (simplifyBank bank names)
            [ makeCustomerWithBalance "Joe" 42
              makeCustomerWithBalance "Bill" 0
              makeCustomerWithBalance "Jane" 10000 ]

        printfn "All tests passed."
        0
    with CheckFailedException e ->
        eprintfn "%s" e
        1
