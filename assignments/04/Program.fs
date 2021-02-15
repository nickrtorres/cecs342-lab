open ListOps

exception CheckFailedException of string

let check expected actual steps =
    if expected <> actual then
        let baseMsg =
            $"Check failed. Expected: '{expected}' got: '{actual}'. Performed the following steps:\n"


        raise (CheckFailedException $"{baseMsg}\t{steps}")

[<EntryPoint>]
let main _ =
    try
        let xs = [ 1; 2; 3; 4; 5; 6; 7; 8; 9; 10 ]
        check 1 (count 2 xs) (sprintf "count 2 %A" xs)
        check 0 (count 42 xs) (sprintf "count 42 %A" xs)

        check 5 (countEvens xs) (sprintf "countEvens %A" xs)

        check 10 (lastElement xs) (sprintf "lastElement %A" xs)

        let allOdds = [ 1; 3; 5; 7; 9 ]
        check 0 (countEvens allOdds) (sprintf "countEvens %A" xs)

        let accounts =
            [ Balance 100
              Balance 300
              Overdrawn 500
              Overdrawn 50
              Overdrawn 1000
              Empty ]

        check 1000 (maxOverdrawn accounts) (sprintf "maxOverdrawn %A" accounts)

        let emptyAccounts = [ Empty; Empty; Empty ]
        check 0 (maxOverdrawn emptyAccounts) (sprintf "maxOverdrawn %A" emptyAccounts)

        let balanceAccounts =
            [ Balance 100
              Balance 200
              Balance 300 ]

        check 0 (maxOverdrawn balanceAccounts) (sprintf "maxOverdrawn %A" balanceAccounts)

        printfn "All tests passed."

        0
    with CheckFailedException e ->
        printfn "%s" e
        1
