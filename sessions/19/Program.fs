open Bank

// The following is an excerpt from the "CECS 342 - Exam 2 review guide" on BeachBoard.
//
// Study Topics
// - What are first-class functions, and what are higher order functions?
// - variable
let square x = x * x

let squareFn = square

// - parameter
let check pred x = pred x
assert check (fun x -> x > 10) 42

// - return
let add (x: int) (y: int) = x + y
let sub (x: int) (y: int) = x - y

type strategy =
    | Add
    | Sub

let dispatch strategy =
    match strategy with
    | Add -> add
    | Sub -> sub

let result = dispatch Add 10 32
assert (result = 42)

// - What is tail recursion, why is it efficient? Be prepared to translate a
//   non-tail-recursive into a tail-recursive equivalent.
let rec count x coll =
    match coll with
    | [] -> 0
    | hd :: tl when hd = x -> 1 + count x tl
    | _ :: tl -> count x tl

let countTail x coll =
    let rec countTail' coll acc =
        match coll with
        | [] -> acc
        | hd :: tl when hd = x -> countTail' tl (acc + 1)
        | _ :: tl -> countTail' tl acc

    countTail' coll 0

assert ((count 42 [ 10; 20; 30; 42; 50; 42 ]) = 2)
assert ((count 42 [ 10; 20; 30; 42; 50; 42 ]) = 2)

// - How do the filter, map, reduce, and fold functions work? What is a
//   predicate; a transform; an aggregate; and a combiner?

// Custom version of List.filter
let myFilter pred coll =
    let rec myFilter' coll acc =
        match coll with
        | [] -> List.rev acc
        | hd :: tl when pred hd -> myFilter' tl (hd :: acc)
        | _ :: tl -> myFilter' tl acc

    myFilter' coll []

assert ((myFilter (fun x -> x % 2 = 0) [ 1; 2; 3; 4; 5; 6 ]) = [ 2; 4; 6 ])

let myMap transform coll =
    let rec myMap' coll acc =
        match coll with
        | [] -> List.rev acc
        | hd :: tl -> myMap' tl (transform hd :: acc)

    myMap' coll []

assert ((myMap (fun x -> x * x) [ 1; 2; 3; 4; 5 ]) = [ 1; 4; 9; 16; 25 ])

let myFold combiner state coll =
    let rec myFold' coll acc =
        match coll with
        | [] -> acc
        | hd :: tl -> myFold' tl (combiner acc hd)

    myFold' coll state

assert (myFold (fun acc elem -> acc + elem) 0 [ 1; 2; 3; 4; 5 ] = 15)

// ('a -> 'b -> 'a) -> 'a -> 'b list -> 'a
// (int -> int list -> int) -> int -> int list list -> int
let result' =
    myFold
        (fun acc elem -> (List.length elem) + acc)
        0
        [ [ 42; 100 ]
          [ 20 ]
          [ 42; 52; 62; 72 ]
          [ 8; 9 ] ]

assert (9 = result')

let myReduce combiner coll =
    myFold combiner (List.head coll) (List.tail coll)

// - Demonstrate the behavior and output of a series of higher order function calls.
assert ((List.filter (fun x -> x % 2 = 0) [ 1; 2; 3; 4; 5; 6; 7; 8; 9 ]
         |> List.map (fun x -> x * x)
         |> List.fold (+) 0) = 120)

// - How do persistent lists work, in particular the cons, tail, append, find,
//   and skip functions?
type myCons =
    | Empty
    | Cons of int * myCons

let myList = Cons(100, Cons(42, Empty))

// - What is early vs late binding in closures? Demonstrate the difference.
// - What is the Seq module in F#?
//
// F#
// - Everything from before; but the emphasis will be on using unions and
//   records, not defining them.
// - I will give you questions featuring the Bank Account types from lab. Study them.
// - You will certainly need to apply the filter, map, reduce, and/or fold functions.
// - You should understand the purpose of the Seq module functions.

[<EntryPoint>]
let main _ = 0
