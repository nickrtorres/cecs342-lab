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
//
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
let isA2 m =
    match m with
    | A -> true
    | _ -> false

// Discriminated unions can also hold data. Below, we define a new type
// constructor called Contact that has two data constructors: Email and Phone.^
//
// ^: This example is from the course lecture notes:
// https://github.com/csulb-cecs342-2021sp/Lectures/blob/master/FSharp/Unions/Program.fs#L16
type Contact =
    | Email of string
    | Phone of int64

let joe = Phone 1234567890L
let alyssa = Email "alyssa@example.com"

let howToContact contact =
    match contact with
    | Email e -> "Email them at " + e
    | Phone p -> "Call them at " + string p

printfn "%s" (howToContact joe)
printfn "%s" (howToContact alyssa)

// Here's another example of holding data in a discriminated union.
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

// You can store multiple values in a data constructor with tuples.
type Number =
    | Natural of int
    | Rational of int * int

// You can also match on tuples using the same `match` syntax.
let addTwoNumbers a b =
    match (a, b) with
    | (Natural n1, Natural n2) -> Natural(n1 + n2)
    | (Rational (p1, q1), Rational (p2, q2)) ->
        if q1 = q2 then
            Rational(p1 + p2, q2)
        else
            Rational(q2 * p1 + q1 * p2, q1 * q2)
    | (Natural n, Rational (p, q))
    | (Rational (p, q), Natural n) -> Rational(1 * p + q * n, q)

let oneHalf = Rational(1, 2)
let oneQuarter = Rational(1, 4)
let four = Natural 4
assert (Rational(6, 8) = (addTwoNumbers oneHalf oneQuarter))
assert (Rational(2, 2) = (addTwoNumbers oneHalf oneHalf))
assert (Rational(9, 2) = (addTwoNumbers four oneHalf))

// Discriminated unions can also refer to themselves. This is called a recursive
// data type. Below, Expr is defined as Add of two Expr's (an augend and an
// addend), Sub of two Expr's (a subtrahend and a minuend), or Int of a single
// atomic number.
//
// You can optionally label the fields within a variant. When you label fields
// you can specify the fields by name or by position. Unlabeled fields rely
// solely on position (just like tuples).
type Expr =
    | Add of augend: Expr * addend: Expr
    | Sub of subtrahend: Expr * minuend: Expr
    | Int of atom: int

// Consider the arithmetic expression: 1 + 2 - 3 + 4. We can represent this
// expression with our data structure above.
//                 Add
//                 / \
//               Sub  4
//               / \
//             Add  3
//             / \
//            1   2
let exp =
    Add(augend = Sub(subtrahend = Add(augend = Int 1, addend = Int 2), minuend = Int 3), addend = Int 4)

// Notice that the example above uses the labels we specified in the declaration
// of Expr. These labels are optional; they act like positional arguments if you
// omit them. Here are a few examples:
//
// Using the labels; order doesn't matter.
let sub1 =
    Sub(minuend = Int(atom = 10), subtrahend = Int(atom = 20))

// Not using the labels; order matters.
let sub2 = Sub(Int 10, Add(Int 2, Int 3))

// Below, we define a function, eval, that evaluates an Expr. It does this by
// recursively evaluating each nested Expr until it reaches an atomic value.
// Don't worry about the recursion too much right now. We'll go into that more
// in depth later.
//
// Note: The `function` keyword allows you to omit `match <expression> with`
// when doing case analysis in a function with a single argument. This is
// syntactic sugar for the equivalent match expression:
//     let rec eval expr =
//         match expr with
//         | ...
let rec eval =
    function
    | Add (augend, addend) -> eval augend + eval addend
    | Sub (subtrahend, minuend) -> eval subtrahend - eval minuend
    | Int atom -> atom

// Evaluating the expression, exp, we defined above looks like this.
//
// eval exp
//
// 1.
//                 Add
//                 / \
//               Sub  4
//               / \
//             Add  3
//             / \
//            1   2
// 2.
//                 Add
//                 / \
//               Sub  4
//               / \
//              3   3
// 3.
//                 Add
//                 / \
//                0   4
// 4.
//                  4
assert (eval exp = 4)

// Just like recursive functions, recursive types need a way to stop. Otherwise
// you'll end up with something that goes on forever.
type MyInfiniteType = Infinity of MyInfiniteType
// let inf = Infinity (Infinity (Infinity ...))

[<EntryPoint>]
let main _ = 0
