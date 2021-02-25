module ListOps

type Account =
    | Balance of int
    | Overdrawn of int
    | Empty

// For the functions described below, first give a comment that expresses the
// type of the function, then provide an implementation that matches the
// description. (Note: make sure you understand the type of the function if your
// editor provides you with the type automatically). All of the functions below
// take lists. Make sure to note when the list can be generic.
//
// The implementation *must* be recursive.
//
// Descriptions below were provided by Mr. Neal Terrell.

// count x coll
//
// count the number of values equal to x in coll.
//
// val count: 'a -> 'a list -> int
let rec count x coll =
    match coll with
    | [] -> 0
    | hd :: tl when hd = x -> 1 + count x tl
    | _ :: tl -> count x tl

// countEvens coll
//
// count the number of even integers in coll.
//
// val countEvens: int list -> int
let rec countEvens coll =
    match coll with
    | [] -> 0
    | hd :: tl when hd % 2 = 0 -> 1 + countEvens tl
    | _ :: tl -> countEvens tl

// lastElement coll
//
// return the last element in the list
//
// val lastElement: 'a list -> 'a
let rec lastElement coll =
    match coll with
    | [] -> failwith "list is empty!"
    | [ x ] -> x
    | _ :: tl -> lastElement tl

// maxOverdrawn coll
//
// given a list of Accounts, return the largest Overdrawn amount, or 0 if none
// are overdrawn
//
// val maxOverdrawn: Account list -> int
let rec maxOverdrawn coll =
    match coll with
    | [] -> 0
    | Overdrawn amount :: tl -> max amount (maxOverdrawn tl)
    | _ :: tl -> maxOverdrawn tl
