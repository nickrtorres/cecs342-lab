module ListOps

// Implement the functions described below -- you must use the option type to
// solve this problem. See the [API docs] for more information.
//
// [API docs]: https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-optionmodule.html
//
// Your implementation should use:
// - pattern matching
// - List.map / List.filter where needed
// - anonymous functions
//
// Your implementation must not use:
// - mutation
// - while loops
// - List.append
// - the '@' operator to concatenate lists
// - if / else

type Account =
    | Overdrawn of int
    | Balance of int
    | Empty

type Customer = { Name: string; Account: Account }

// Same prompt as before:
// Given a list of customers and a list of names, simplify the list of customers
// such that each name in the string list only appears in the final customer
// list one time.
//
// ... but this time the names might not exist in the Customer list. How can you
// solve this problem with the option type?
//
// let bank =
//     [ { Name = "Jane"; Account = Balance 100 }
//       { Name = "Joe"; Account = Overdrawn 200 }
//       { Name = "Jane"; Account = Overdrawn 100 }
//       { Name = "Joe"; Account = Empty }
//       { Name = "Jane"; Account = Balance 100 }
//       { Name = "Joe"; Account = Balance 50 } ]
//
// let names = [ "Unknown customer"; "Jane"; "Joe" ]
//
// assert ((simplifyBank bank names) = ([ { Name = "Jane"; Account = Balance 100 }
//                                        { Name = "Joe"; Account = Overdrawn 150 } ]))
val simplifyBank : Customer list -> string list -> Customer list
