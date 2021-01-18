open System
open System.Text.RegularExpressions

exception LexicalException of string
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

let identifierFromToken token =
    match token with
    | Identifier s -> s
    | _ -> failwith "invalid access!"

let lex (source: string) =
    let tokenized = source.Split()
    let isNumber d = Regex.IsMatch(d, "[0-9]+")

    let isIdentifier s =
        Regex.IsMatch(s, "[a-zA-Z][a-zA-Z0-9_]*")

    let fromStr token =
        match token with
        | "=" -> Eq
        | "let" -> Let
        | "-" -> Minus
        | "print" -> Print
        | "+" -> Plus
        | ";" -> Semicolon
        | d when isNumber d -> Num(Convert.ToInt32 d)
        | s when isIdentifier s -> Identifier s
        | s -> raise (LexicalException(s))

    Array.map fromStr tokenized |> Array.toList

type Expr =
    | Num of int
    | Identifier of string

type Stmt =
    | LetStmt of string * Expr
    | PrintStmt of Expr

type StmtList =
    | SingleStmt of Stmt
    | CompoundStmt of Stmt * StmtList

let parse tokens =
    let isMatch exp act =
        match (exp, act) with
        | (Token.Identifier _, Token.Identifier _)
        | (Token.Num _, Token.Num _) -> true
        | (e, a) -> e = a

    let eat tokens exp =
        if (isMatch (List.head tokens) exp) then (List.head tokens, List.tail tokens) else failwith "unexpected token!"

    let rec program tokens = stmtList tokens

    and stmtList tokens =
        match stmt tokens with
        | (s, []) -> SingleStmt s
        | (s, tl) ->
            let (_, afterSemi) = eat tl Semicolon
            CompoundStmt(s, stmtList afterSemi)

    and stmt tokens =
        match List.head tokens with
        | Let -> letStmt tokens
        | Print -> printStmt tokens
        | t -> raise (SyntaxException(sprintf "Invalid start of statement: '%A'" (t)))

    and expression tokens =
        match List.head tokens with
        | Token.Num n -> (Num n, List.tail tokens)
        | Token.Identifier s -> (Identifier s, List.tail tokens)
        | t -> raise (SyntaxException(sprintf "Invalid start of expression: '%A'" (t)))

    and letStmt tokens =
        let _, afterLet = eat tokens Let
        let iden, afterIden = eat afterLet (Token.Identifier "")
        let _, afterEq = eat afterIden Eq
        let exp, afterExpr = expression afterEq
        LetStmt(identifierFromToken iden, exp), afterExpr

    and printStmt tokens =
        let _, afterPrint = eat tokens Print
        let exp, afterExpr = expression afterPrint
        PrintStmt(exp), afterExpr

    program tokens

[<EntryPoint>]
let main argv =
    let ok = 0
    let err = 1

    match Array.length argv with
    | 1 ->
        try
            printfn "%A" (Array.head argv |> lex |> parse)
            ok
        with
        | LexicalException e ->
            eprintfn "Lexical error at: %s" e
            err
        | SyntaxException e ->
            eprintfn "Syntax error at: %s" e
            err
    | _ ->
        eprintfn "usage: Arithmos '<program>'"
        ok
