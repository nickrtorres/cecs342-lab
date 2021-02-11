module Bank

type Account =
    | Balance of int
    | Overdrawn of int
    | Empty

type Password = string

type Name = string

type Customer =
    { Name: Name
      Password: Password
      Account: Account }

type Action =
    | Withdraw of int
    | Deposit of int

type Session =
    | Valid of Customer
    | BadPassword

type TransactionResult =
    | AccountUpdated of Customer
    | Failed

let makeAccount () = Empty

let makeCustomer name password =
    { Name = name
      Password = password
      Account = makeAccount () }

let withdraw amount acc =
    match acc with
    | Empty -> Overdrawn amount
    | Overdrawn n -> Overdrawn(amount + n)
    | Balance n when n - amount > 0 -> Balance(n - amount)
    | Balance n when n - amount < 0 -> Overdrawn(abs (n - amount))
    | Balance _ -> Empty

let deposit amount acc =
    match acc with
    | Empty -> Balance amount
    | Balance n -> Balance(amount + n)
    | Overdrawn n when n - amount > 0 -> Overdrawn(n - amount)
    | Overdrawn n when n - amount < 0 -> Balance(abs (n - amount))
    | Overdrawn _ -> Empty

let makeSession password customer =
    if password <> customer.Password then
        BadPassword
    else
        Valid customer

let performTransaction txn session =
    match session with
    | BadPassword -> Failed
    | Valid customer ->
        match txn with
        | Withdraw amount ->
            AccountUpdated
                { customer with
                      Account = withdraw amount customer.Account }
        | Deposit amount ->
            AccountUpdated
                { customer with
                      Account = deposit amount customer.Account }
