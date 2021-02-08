open Bank

exception CheckFailedException of string

let check actual expected steps =
    if actual <> expected then
        let baseMsg =
            $"Check failed. Expected: '{expected}' got: '{actual}'. Performed the following steps:\n"

        let s =
            List.fold (fun acc elem -> $"{acc}\t{elem}\n") "" steps

        raise (CheckFailedException $"{baseMsg}{s}")

[<EntryPoint>]
let main argv =
    try
        let txn1 =
            makeAccount ()
            |> deposit 100
            |> deposit 100
            |> withdraw 150

        check
            txn1
            (Balance 50)
            [ "Empty"
              "Deposit 100"
              "Deposit 100"
              "Withdraw 150" ]

        let txn2 =
            makeAccount ()
            |> deposit 100
            |> deposit 100
            |> withdraw 300

        check
            txn2
            (Overdrawn 100)
            [ "Empty"
              "Deposit 100"
              "Deposit 100"
              "Withdraw 300" ]

        let txn3 =
            makeAccount ()
            |> deposit 100
            |> deposit 100
            |> withdraw 300
            |> deposit 101

        check
            txn3
            (Balance 1)
            [ "Empty"
              "Deposit 100"
              "Deposit 100"
              "Withdraw 300"
              "Deposit 101" ]

        let txn4 =
            makeAccount () |> deposit 0 |> withdraw 0

        check txn4 (Empty) [ "Empty"; "Deposit 0"; "Withdraw 0" ]

        let janeTxn =
            makeCustomer "jane" "1234"
            |> makeSession "1234"
            |> performTransaction (Deposit 100)

        check
            janeTxn
            (AccountUpdated(
                { Name = "jane"
                  Password = "1234"
                  Account = Balance 100 }
            ))
            [ "makeCustomer"
              "makeSession"
              "performTransaction (Deposit 100)" ]

        let joeTxn =
            makeCustomer "joe" "1234"
            |> makeSession "abcd"
            |> performTransaction (Deposit 100)

        check
            joeTxn
            Failed
            [ "makeCustomer"
              "makeSession (invalid password)"
              "performTransaction (Deposit 100)" ]

        printfn "All tests passed."
        0
    with CheckFailedException s ->
        printfn "%s" s
        1
