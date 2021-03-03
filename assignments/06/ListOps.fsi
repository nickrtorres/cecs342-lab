module ListOps

type Account =
    | Balance of int
    | Overdrawn of int
    | Empty

type Customer = { Name: string; Account: Account }

val unknownCustomer: Customer

val makeCustomerWithBalance: string -> int -> Customer

// Implement the functions below using List.{map,filter} where applicable.
//
// To receive credit your implementation should:
// - Use anonymous functions where applicable
// - Use map / filter where applicable
// - Use match / cons to deconstruct lists into its head and tail components
//
// Your implementation *should not*:
// - Use mutable variables or loops
// - Use List.length or List.empty instead of match

// Given a list of customers, find the total overdrawn amount for a customer
// with a given name. If the customer does not have any overdrawn accounts, then
// return 0.
val totalOverdrawn: string -> Customer list -> int

// Given a list of customers, find the maximum balance for a customer with a
// given name. If the customer does not have a positve balance, then return
// unknownCustomer.
val maxBalance: string -> Customer list -> Customer
