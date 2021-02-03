// Discriminated unions (sum types, variants, tagged unions, enums, etc.) are
// awesome. The following examples provide an overview to their use in F#.
//
// Additional resources are available in the course notes [1] and in Microsoft's
// F# docs [2].
//
// [1]: https://github.com/csulb-cecs342-2021sp/Lectures/blob/master/FSharp/Unions/Program.fs
// [2]: https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/discriminated-unions

// In F#, discriminated unions consist of two elements: a type constructor, i.e., the
// name of the type, and a set of data constructors that specify the forms that
// a type constructor can take. The simplest form of an F# discriminated union
// is a C-style enum.
type MyType =
    | A
    | B
    | C
    | D

// A, B, C, and D are data constructors for MyType. A value of MyType represents
// exactly one of these variants. F#'s convention is to use PascalCase for the
// constructors and the type name.
//
// Declaring a value of type MyType is done by selecting a single constructor
// from MyType's set of data constructors.
let a = A

// F# provides a match expression that lets you perform case analysis on a
// variant type (and its contents (but more on that later)). Match expressions use
// a hidden tag that is embedded into the type by the compiler -- this is the part that
// makes an F# union discriminated. The syntax for a match expression from the
// MS F# docs is given below // below.
//
//     match test-expression with
//     | pattern1 -> result-expression1
//     | pattern2 -> result-expression2
//     | ... // //
//
// [1]: https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/match-expressions):
//
// Below, isA performs case analysis on a value of type MyType. Each case or arm
// in the match expression is inspected and the first fit is chosen.
//
// There is only one possible answer since MyType is a simple enumeration.
let isA m =
    match m with
    | A -> true
    | B -> false
    | C -> false
    | D -> false

// You can consolidate match arms if their rhs is the same by separating the lhs
// data constructors with vertical bars e.g.
//
//     let isA m =
//         match m with
//         | A -> true
//         | B | C | D -> false

assert isA a

// Match expressions must be exhaustive. The F# compiler will warn you if miss
// one of the data constructors for your type.
let isB m =
    match m with
    | A -> false
    | B -> true

// The F# runtime will throw a 'MatchFailureException' if you ignore this warning
// and pass a value that hits an unhandled case. e.g.:
//
// let c = C
// isB c // yikes!

// You can also use the "don't care" syntax you learned to ignore certain data
// constructors. This will handle the warning above, but it comes with a cost.
let isA2 m =
    match m with
    | A -> true
    | _ -> false

// The F# compiler can no longer warn you if you miss a case when you use '_'.
// Consider a case where we decide to add a new data constructor to MyType that
// should also return true for isB (a very intuitive procedure name).
//
// type MyType =
//     | A
//     | B
//     | C
//     | D
//     | AlsoCountsAsB
//
// Now our isB procedure will silently fail without any warning from the compiler!
//
//
// Discriminated unions can also hold data in the form of tuples.
//
// Below, SafeDivisionResult holds the result of performing "safe" division on
// two numbers. The result of "safe" division is defined below.
//
//                 +-- Quotient q such that q = x / y iff y != 0
//       x / y =  -+
//                 +-- Undefined otherwise
//
type SafeDivisionResult =
    | Quotient of int
    | Undefined

let safeDivide dividend divisor =
    if divisor = 0 then
        Undefined
    else
        Quotient(dividend / divisor)

assert (Quotient 21 = safeDivide 42 2)
assert (Undefined = safeDivide 1 0)

// Discriminated unions can also refer to themselves. This is called a recursive
// data type. Below, LinkedList is defined as either Empty or a Cons of an
// integer (the head) and another LinkedList (the tail).
//
// NB: The word 'Cons' [1] is ubiquitous in many functional programming
// languages. It stands for "construct" and is a primitive procedure in many
// functional languages (F# also has a built-in cons operator!).
//
// [1]: https://en.wikipedia.org/wiki/Cons
type LinkedList =
    | Empty
    | Cons of hd: int * tl: LinkedList

// The list below has the following structure.
//
//      let mylist = Cons (1, Cons (2, Cons (3, Empty)))
//          |
//          |
//          V
//          +-+    +-+    +-+
//          |1|--->|2|--->|3|---> Empty
//          +-+    +-+    +-+
let mylist = Cons(1, Cons(2, Cons(3, Empty)))

// Adding an item to a list is as simple as 'cons'ing the new element with the
// old list. This operation is not destructive. Instead, it returns a new list
// with 'x' appended to the front.
let add x xs = Cons(x, xs)

// Checking for membership in a list _can_ look just like an imperative
// language, i.e., iterating through the list with a pointer to the current
// element. There are nicer ways to do this, but more on those later.
//
// Note the first arm in the match statement below. The `when <boolean-expression>`
// is called a guard. The pattern must match and the guard expression must
// evaluate to true for this arm to be chosen.
let contains x xs =
    let mutable eol = false
    let mutable found = false
    let mutable current = xs

    while not found && not eol do
        match current with
        | Cons (hd, _) when hd = x -> found <- true
        | Cons (_, tl) -> current <- tl
        | Empty -> eol <- true

    found

let mutable xs = Empty
xs <- add 50 xs
xs <- add 40 xs
xs <- add 30 xs
xs <- add 20 xs
xs <- add 10 xs

assert (contains 50 xs)
assert (contains 10 xs)
assert (not (contains 42 xs))

// Just like recursive functions, recursive types need a way to stop. Otherwise
// you'll end up with something that goes on forever.
type MyInfiniteType = Infinity of MyInfiniteType
// let inf = Infinity (Infinity (Infinity ...))

// You can optionally label the fields within a variant. When you label fields
// you can specify the fields by name or by position. Unlabeled fields rely
// solely on position (just like tuples).
type Expr =
    | Add of augend: Expr * addend: Expr
    | Sub of subtrahend: Expr * minuend: Expr
    | Int of atom: int

// Using the labels; order doesn't matter.
let sub1 =
    Sub(minuend = Int 10, subtrahend = Int 20)

// Not using the labels; order matters.
let sub2 = Sub(Int 10, Add(Int 2, Int 3))

// The `function` keyword allows you to omit `match <expression> with` when
// doing case analysis in a function with a single argument. This is syntactic
// sugar for the equivalent match expression:
//     let rec eval expr =
//         match expr with
//         | ...
let rec eval =
    function
    | Add (augend, addend) -> eval augend + eval addend
    | Sub (subtrahend, minuend) -> eval subtrahend - eval minuend
    | Int atom -> atom

assert (eval sub1 = 10)
assert (eval sub2 = 5)

[<EntryPoint>]
let main _ = 0
