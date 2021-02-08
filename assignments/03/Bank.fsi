module Bank

// Your goal is to implement a group of functions that will model a bank,
// namely, a customer's interaction with their account.
//
// The flow of information through the bank is given below.
// 1. Make an account for a customer with a name and password (see `makeCustomer`)
// 2. Make a session so that the customer can make transactions (see `makeSession`)
//      - `makeSession` needs to make sure that the customer's password is correct
//        before returning a valid session. If the password is not correct, this
//        function must return `BadPassword`.
// 3. Perform any number of transaction (see performTransaction).
//      - A successful transaction will return a `TransactionResult` with a modified
//        customer as its data. A transaction is only valid if a valid session is
//        provided to `performTransaction`.

// An account has three variants:
// - Balance
// - Overdrawn
// - Empty
//
// Empty represents an account with $0. Balance and Withdrawn represent the
// absolute value of funds in a customer's account. For example, if a customer's
// account starts with a `Balance 5` and they try to withdraw $10, then their
// account will be represented as `Overdrawn 5`.
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

// Makes an Empty account.
val makeAccount: unit -> Account

// Withdraws the given amount from an account.
val withdraw: int -> Account -> Account

// Deposits the given amount into an account.
val deposit: int -> Account -> Account

// Makes a customer with the given name and password.
val makeCustomer: Name -> Password -> Customer

// Makes a session for a Customer. A session is `Valid` iff the password given
// matches the customer's password. `BadPassword` is returned otherwise.
val makeSession: Password -> Customer -> Session

// Performs the specified action on a customer's account and returns an updated
// customer iff the session is valid. Returns `Failed` otherwise.
val performTransaction: Action -> Session -> TransactionResult
