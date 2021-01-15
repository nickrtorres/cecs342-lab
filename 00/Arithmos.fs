open System
open System.Text.RegularExpressions

exception SyntaxException of string

type Token =
    | Eq
    | Identifier of string
    | Let
    | Num of int
    | Print
    | Plus
    | Minus
    | Semicolon

let lex (source: string) =
    let tokenized = source.Split()
    let isNumber d = Regex.IsMatch(d, "[0-9]+")

    let isIdentifier s =
        Regex.IsMatch(s, "[a-zA-Z][a-zA-Z0-9_]*")

    let fromStr token =
        match token with
        | " " -> None
        | "=" -> Some Eq
        | "let" -> Some Let
        | "-" -> Some Minus
        | "print" -> Some Print
        | "+" -> Some Plus
        | ";" -> Some Semicolon
        | d when isNumber d -> Convert.ToInt32 d |> Num |> Some
        | s when isIdentifier s -> Identifier s |> Some
        | s -> raise (SyntaxException(s))

    Array.map fromStr tokenized |> Array.choose id

assert ([| Let
           Identifier "x"
           Eq
           Num 1
           Semicolon |] =
    lex "let x = 1 ;")

[<EntryPoint>]
let main argv =
    try
        printfn "%A" (Array.head argv |> lex)
    with SyntaxException e -> eprintfn "Syntax error at '%s'" e

    0 // return an integer exit code
