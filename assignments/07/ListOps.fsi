module ListOps

// Implement the functions described below making use of List.fold at least once
// in each function.
//
// Your implementation should use:
// - pattern matching
// - List.map, List.filter, and / or List.reduce
// - anonymous functions
//
// Your implementation should not use:
// - mutation
// - while loops
// - List.append
// - the '@' operator to concatenate lists


// Given a list of strings and a separator, create a new string that is
// delimited with the separator (in the style of str.join from python).
//
// Example:
// assert ("this is a sentence." = join " " [ "this"; "is"; "a"; "sentence." ])
val join : string -> string list -> string

type Account =
    | Overdrawn of int
    | Balance of int
    | Empty

type Customer = { Name: string; Account: Account }

// Given a list of customers and a list of names, simplify the list of customers
// such that each name in the string list only appears in the final customer
// list one time.
//
// For example, if Jane appears in the customer list 3 times as:
//   { Name = "Jane"; Account = Balance 100 }
//   { Name = "Jane"; Account = Overdrawn 100 }
//   { Name = "Jane"; Account = Balance 100 }
//
// Jane's account can be simplified to:
//   { Name = "Jane"; Account = Balance 100 }
//
// Full example:
// let bank =
//     [ { Name = "Jane"; Account = Balance 100 }
//       { Name = "Joe"; Account = Overdrawn 200 }
//       { Name = "Jane"; Account = Overdrawn 100 }
//       { Name = "Joe"; Account = Empty }
//       { Name = "Jane"; Account = Balance 100 }
//       { Name = "Joe"; Account = Balance 50 } ]
//
// let names = [ "Jane"; "Joe" ]
//
// assert ((simplifyBank bank names) = ([ { Name = "Jane"; Account = Balance 100 }
//                                        { Name = "Joe"; Account = Overdrawn 150 } ]))
val simplifyBank : Customer list -> string list -> Customer list
