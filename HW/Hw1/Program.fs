open System

// How do we write a function that doesn't take any arguments?
//
// Example:
// How would we write a function called `greet` that writes "Hello!" to the
// console, but doesn't take any arguments?
//
// What would the type signature be for that function?
// greet ??? -> ???
//
// Zero argument functions use the unit type. greet's implementation is given
// below. It takes *exactly* 1 argument (a unit) and calls printfn.
//
// Calling greet looks like this:
// greet ()
//
// There is a subtle distinction here between F# and Java, C, etc. The
// parentheses do not invoke the function, instead we are passing `greet` a
// single parameter: i.e. a unit.
let greet () = printfn "hello !"

// Random number generation uses the .NET library. You create a `Random` object
// by writing `Random ()`. Once you have an object you can call methods on it
// similar to Java or C#.
//
// e.g.
//   Random().Next(1, 10) will create a random integer in the range [1, 10).
//
// Random API documentation: https://docs.microsoft.com/en-us/dotnet/api/system.random?view=net-5.0
let getRandomNumber upper = Random().Next(1, upper)

[<EntryPoint>]
let main argv =
    greet ()
    printfn "Random number is: %d" (getRandomNumber 10)
    0
