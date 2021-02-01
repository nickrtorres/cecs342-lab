// This overview references the class notes [1] and the microsoft F# guide [2].
// [1]: https://github.com/csulb-cecs342-2021sp/Lectures/blob/master/FSharp/Records/Program.fs
// [2]: https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/records
//
// Records are declared with the `type` keyword and curly braces. The convention
// for field names is to begin with uppercase letters.
//
// Record fields are separated by semicolons ...
type Point = { X: int; Y: int }

// ... or new lines.
type Language =
    { Name: string
      FirstAppeared: int
      Creator: string }

// Declaring a value of a record type uses the `let` keyword. This is called a
// record expression.
let myPoint = { X = 42; Y = 100 }

let cpp =
    { Name = "C++"
      FirstAppeared = 1985
      Creator = "Bjarne Stroustrup" }

// F# records support field access through dot notation (just like C, Java, etc ).
let cppYear = cpp.FirstAppeared

// Alternatively, you can deconstruct a record (just like Python, Rust, etc.).
let { X = x; Y = y } = myPoint

// You can even ignore fields you don't care about.
let { Creator = cppCreator
      Name = _
      FirstAppeared = _ } =
    cpp

// The order of the fields in a record expression does not matter.
let pascal =
    { Creator = "Niklaus Wirth"
      Name = "Pascal"
      FirstAppeared = 1970 }

let flowMatic =
    { Name = "FLOW-MATIC"
      Creator = "Grace Hopper"
      FirstAppeared = 1955 }

// Records can be used as function arguments.
let printLanguageDetails language =
    printfn "%s first appeared in %d and was created by %s." language.Name language.FirstAppeared language.Creator

// An existing record can be used as a starting point to create a new record
// using the `with` keyword.
//
// This called a "copy and update" record expression.
let modernCpp =
    { cpp with
          Name = "C++11"
          FirstAppeared = 2011 }

[<EntryPoint>]
let main argv =
    printfn "%s" cppCreator

    printLanguageDetails pascal
    printLanguageDetails flowMatic
    0 // return an integer exit code
