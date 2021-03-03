open ListOps

exception CheckFailedException of string

let check actual expected =
    if actual <> expected then
        raise (CheckFailedException(sprintf "Check failed. Expected: '%A' got: '%A'." expected actual))
    else
        ()

[<EntryPoint>]
let main argv =
    try
        let customers =
            [ makeCustomerWithBalance "Joe" 100
              makeCustomerWithBalance "Jane" 1500
              makeCustomerWithBalance "Joe" 600
              makeCustomerWithBalance "Joe" 200
              makeCustomerWithBalance "Joe" 800
              makeCustomerWithBalance "Joe" -500
              makeCustomerWithBalance "Joe" 300
              makeCustomerWithBalance "Joe" -400
              makeCustomerWithBalance "Jane" 1000
              makeCustomerWithBalance "John" -1500
              makeCustomerWithBalance "John" -500
              makeCustomerWithBalance "Joe" -400
              makeCustomerWithBalance "Joe" -800 ]

        check (maxBalance "Joe" []) unknownCustomer
        check (maxBalance "Joe" customers) (makeCustomerWithBalance "Joe" 800)
        check (maxBalance "Jane" customers) (makeCustomerWithBalance "Jane" 1500)
        check (maxBalance "John" customers) unknownCustomer

        let noOverdrawnCustomers =
            [ makeCustomerWithBalance "Jane" 1500
              makeCustomerWithBalance "Joe" 600
              makeCustomerWithBalance "Joe" 200
              makeCustomerWithBalance "Joe" 800 ]

        check (totalOverdrawn "Joe" []) 0
        check (totalOverdrawn "Joe" noOverdrawnCustomers) 0
        check (totalOverdrawn "John" customers) 2000

        printfn "All tests passed."
        0
    with CheckFailedException e ->
        eprintfn "%s" e
        1
