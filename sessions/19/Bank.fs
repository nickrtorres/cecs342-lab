module Bank

type Account =
    | Overdrawn of int
    | Balance of int
    | Empty

type Customer = { Name: string; Account: Account }
