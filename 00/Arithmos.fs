open System
open System.Text.RegularExpressions

exception SyntaxException of string
exception SemanticException of string

type Token =
    | Eq
    | Identifier of string
    | Let
    | Num of int
    | Print
    | Plus
    | Minus
    | Semicolon

let identifierFromToken token =
    match token with
    | Identifier s -> s
    | _ -> failwith "Invalid access!"

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

    Array.map fromStr tokenized
    |> Array.choose id
    |> Array.toList

type Expr =
    | Num of int
    | Identifier of string

type Stmt =
    | LetStmt of string * Expr
    | PrintStmt of Expr
    | CompoundStmt of Stmt * Stmt

let parse tokens =
    let matches (exp) (act) =
        match (exp, act) with
        | (Token.Identifier _, Token.Identifier _)
        | (Token.Num _, Token.Num _) -> true
        | (e, a) -> e = a

    let eat tokens exp =
        if (matches (List.head tokens) exp)
        then (List.head tokens, List.tail tokens)
        else raise (SemanticException(exp.ToString()))

    let rec stmt tokens =
        let (node, tl) =
            match List.head tokens with
            | Let -> letStmt tokens
            | Print -> printStmt tokens
            | t -> raise (SemanticException(t.ToString()))

        if List.isEmpty tl then
            node
        else
            let (_, afterSemi) = eat tl Semicolon
            CompoundStmt(node, stmt afterSemi)

    and expression tokens =
        match List.head tokens with
        | Token.Num n -> (Num n, List.tail tokens)
        | Token.Identifier s -> (Identifier s, List.tail tokens)
        | t -> failwith (sprintf "Invalid start of expression: %s" (t.ToString()))

    and letStmt tokens =
        let (_, afterLet) = eat tokens Let
        let (iden, afterIden) = eat afterLet (Token.Identifier "")
        let (_, afterEq) = eat afterIden Eq
        let exp, afterExp = expression afterEq
        (LetStmt(identifierFromToken iden, exp), afterExp)

    and printStmt tokens =
        let (_, afterPrint) = eat tokens Print
        let exp, afterExp = expression afterPrint
        (PrintStmt(exp), afterExp)

    stmt tokens

[<EntryPoint>]
let main argv =
    try
        printfn "%A" (Array.head argv |> lex |> parse)
    with
    | SyntaxException e -> eprintfn "Syntax error at: '%s'" e
    | SemanticException e -> eprintfn "Semantic error at: %s" e

    0 // return an integer exit code
