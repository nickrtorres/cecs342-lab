module ListOps

type Account =
    | Balance of int
    | Overdrawn of int
    | Empty

type Customer = { Name: string; Account: Account }

let makeCustomerWithBalance name (amount: int) =
    let account =
        if amount > 0 then
            Balance amount
        elif amount < 0 then
            Overdrawn(abs amount)
        else
            Empty

    { Name = name; Account = account }

let unknownCustomer = makeCustomerWithBalance "Unknown" 0

let totalOverdrawn name customers =
    let netOverdrawn =
        function
        | { Name = _; Account = Overdrawn n } -> n
        | _ -> 0

    match customers with
    | [] -> 0
    | _ ->
        List.filter (fun e -> e.Name = name) customers
        |> List.map netOverdrawn
        |> List.reduce (+)

let maxBalance name customers =
    let filterCustomer =
        function
        | { Name = n; Account = Balance _ } when n = name -> true
        | _ -> false

    let customerReduction acc cust =
        match (acc.Account, cust.Account) with
        | Balance bLeft, Balance bRight when bLeft < bRight -> cust
        | _ -> acc

    match List.filter filterCustomer customers with
    | [] -> unknownCustomer
    | customers -> List.reduce customerReduction customers
