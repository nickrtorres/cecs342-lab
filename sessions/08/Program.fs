// F# review based on the topics Mr. Neal Terrell posted on BeachBoard.
//
// It's pretty roughly formatted and probably will not make much sense if you
// were not in the lab. There is a recording of the lab on BeachBoard.
//

// ## Syntax: variables, functions, type annotations, match, if expressions.
let x = 42

let f (x: int) = x * x

let f' x: int = x * x

// return true if parameter is 42
let g x =
    match x with
    | 42 -> true
    | _ -> false

// or with if
// let g' x =
//     if x = 42 then true else false

// ## Types: int, float, bool. Tuples, lists.
let myint: int = int 42

let myfloat: float = 10.

let mybool = false

// we want a new type that is a tuple of int and float
//
// tuples are product types -> * is for computing for products
// unions are sum types     -> | is for specifying or
type MyTuple = int * float

let addMyTuple (t: MyTuple) =
    let x, y = t
    float x + y

let t1: MyTuple = (1, 2.0)
let addResult = addMyTuple t1

// This does not work. You need a cast *somewhere*.
// let addition = myint + myfloat

let mylist = [ 1; 2; 3 ]

let rec printMyList coll =
    match coll with
    | h :: t ->
        printfn "%A" h
        printMyList t
    | [] -> ()

let () = printMyList mylist
let () = printMyList [ "foo"; "bar"; "baz" ]

// ## Type inference: this is a huge deal in F#, you should be able to identify
// types in the absence of annotations, the same way a compiler would.

// ## Tuples:
//     constructing
//     unpacking
//     type annotations
let t2 = ("foo", 42)

// Bind an identifier named foo to the value "foo" in t2. Do not bind a value to 42.
let (foo: string), _ = t2
let (foo': string) = fst t2

// ## Records:
//     declaring a record
//     constructing a value of a record
//     using the fields of a record
//
// How do you specify a record with two fields: X and Y where X is an int and Y
// is a string?
type MyRecord = { X: int; Y: string }

let myrecord = { X = 5; Y = "word" }
let valueOfX = myrecord.X

// ## Unions:
//     declaring a union
//     constructing an instance of a union
//     pattern matching unions
// Lists:
//     list literals, e.g., [1; 2; 3]
//     using List.head and List.tail; and being comfortable with what a "head" and "tail" is.
//     let x = [1; 2; 3]
//     let y = List.tail x
//     let z = List.head x
//     what type are x, y, and z? be specific.
//     pattern matching with [x] and ::
// recursion -- you will be asked to write a recursive function to do something with a list.
//
// ## Generics:
//     the 'a type
//     identifying a generic parameter
//     determining function types given generic parameters
let echo x = printfn "%A" x

echo "foo"
echo 42
echo [ 1; 2; 3; 4; 5 ]

// Not allowed since all paths throw control flow *must* have the same type.
//
// let matchSomething x =
//     match x with
//     | 1 -> 42
//     | 2 -> 200
//     | _ -> "i don't know"

// Why is this not generic (for the type within the list)?
let doSomething x =
    match x with
    | h :: t -> h
    | [] -> 42

// Why is this generic (for the type within the list)?
let doSomething' x =
    match x with
    | [ x; y ] -> 0
    | h :: l -> 0
    | _ -> 0

// ## Other stuff:
//     the pipe forward operator |>
//
//     conversions with int, float
//
//     You won't be writing output statements, but you might have to read them. %d
//     for integer, %f for float.
//
//     You will not be expected to use mutable variables to solve problems. In fact,
//     some problems will explicitly forbid it.
//
// This is left as an exercise for the reader.

// ##
//
// 6. Write the following F# functions. You can use the built-in List.head and
// List.tail functions, but no other helpers from the List type. Most will
// require recursion:

//
//     (a) compare x y: given two integers x and y, returns a negative -1
//     if x < y, 0 if x == 0, or a +1 if x > y.
//
let compare (x: int) (y: int) =
    if x < y then -1
    elif x > y then 1
    else 0

//
//     (b) secondToLast coll: return the second-to-last element of the given
//     list. Don't worry about lists with 0 or 1 elements 􏰂 or throw an
//     exception, as shown in lab.
//
let rec secondToLast coll =
    match coll with
    | [] -> failwith "Invalid list!"
    | [ x; y ] -> x
    | hd :: tl -> secondToLast tl

//
//     (c) skip n coll: given a counter and a list, return whatever remains in
//         the list after skipping past the first n elements. Think recursively:
//         i. if n is 0, nothing needs to be skipped, and the entire list is returned.
//         ii. if n is positive but the list is empty, return the empty list.
//         iii. otherwise, the result of skipping n eleemnts from the list is
//              the same as skipping n−1 elements from the tail of the list.
//
// This one is left as an exercise for the reader.
//

// ##
//
// 7. Create an F# union to represent a BinaryTree. A BinaryTree has two cases/constructors: it is either
//     (a) Empty; or
//     (b) Node of value:int * left:Node * right:Node
//         that is, a tree is either empty, or it has a node with a value, and a subtree
//         to its left and right. Con- struct an example tree to demonstrate the union,
//         for example
//             Node(5, Node(3, Empty, Node(4, Empty, Empty)), Node(10, Node(6, Empty, Empty), Node(15, Node(11, Empty, Empty), Empty)))
//             giving a small binary search tree with values 5, 3, 4, 19, 6, 15, and 11.
type BinaryTree =
    | Empty
    | Node of int * BinaryTree * BinaryTree

// ##
//
// 8. Write the following functions using your BinaryTree union:
//     (a) isEmpty tree: returns true if the tree is empty, false otherwise.
//     (c) bstContains x tree: suppose that tree is a binary search tree. Search the
//         tree for x and return true if it exists. Again.... recursion.... The empty
//         tree does not contain x. A tree with a node contains x if the node's value is
//         x; OR if x is contained to the left or right of the node, depending on the
//         comparison between x and the node's value.

let myCompleteTree =
    Node(
        10,
        Node(5, Node(1, Empty, Empty), Node(7, Empty, Empty)),
        Node(15, Node(12, Empty, Empty), Node(20, Empty, Empty))
    )
//                         Node (10)
//                             |
//                             +
//           Node(5)                    Node (15)
//             |                            |
//             +                            +
//  Node (1)       Node (7)     Node (12)       Node (20)
//      |              |             |              |
//      +              +             +              +
// Empty Empty    Empty Empty   Empty Empty    Empty Empty

let myEmptyTree = Empty

//
//     (b) countNodes tree: returns the number of nodes in the tree. Again, think
//         recursively: there are 0 nodes in an empty tree; in all other trees, there is
//         1 node plus the count of nodes in the left and right subtrees.
//
let isEmpty tree =
    match tree with
    | Empty -> true
    | Node (x, left, right) -> false

assert isEmpty myEmptyTree
assert (isEmpty myCompleteTree |> not)

let rec countNodes tree =
    match tree with
    | Empty -> 0
    | Node (_, left, right) -> 1 + countNodes left + countNodes right

assert (0 = countNodes myEmptyTree)
assert (7 = countNodes myCompleteTree)

let rec bstContains x tree =
    match tree with
    | Empty -> false
    | Node (v, left, _) when v > x -> bstContains x left
    | Node (v, _, right) when v < x -> bstContains x right
    | Node (v, _, _) -> true

assert bstContains 20 myCompleteTree
assert bstContains 12 myCompleteTree
assert bstContains 1 myCompleteTree
assert bstContains 7 myCompleteTree
assert (bstContains 42 myCompleteTree |> not)
assert (bstContains 42 myEmptyTree |> not)

[<EntryPoint>]
let main argv = 0
