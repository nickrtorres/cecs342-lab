open System
open System.Text.RegularExpressions

exception LexicalException of string
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
        match tokens with
        | hd :: tl when isMatch exp hd -> (hd, tl)
        | hd :: _ -> raise (SyntaxException(sprintf "unexpected token: expected: '%A'; got: '%A'" exp hd))
        | [] -> raise (SyntaxException(sprintf "unexpected eof"))

    let rec program tokens = stmtList tokens

    and stmtList tokens =
        match stmt tokens with
        | (s, []) -> SingleStmt s
        | (s, tl) ->
            let _, afterSemi = eat tl Semicolon
            CompoundStmt(s, stmtList afterSemi)

    and stmt (tokens: Token list) =
        match tokens with
        | Let :: tl -> letStmt tl
        | Print :: tl -> printStmt tl
        | t -> raise (SyntaxException(sprintf "invalid start of statement: '%A'" (t)))

    and expression tokens =
        match tokens with
        | Token.Num n :: tl -> (Num n, tl)
        | Token.Identifier s :: tl -> (Identifier s, tl)
        | t -> raise (SyntaxException(sprintf "invalid start of expression: '%A'" (t)))

    and letStmt tokens =
        let iden, afterIden = eat tokens (Token.Identifier "")
        let _, afterEq = eat afterIden Eq
        let expr, afterExpr = expression afterEq
        LetStmt(identifierFromToken iden, expr), afterExpr

    and printStmt tokens =
        let expr, afterExpr = expression tokens
        PrintStmt(expr), afterExpr

    program tokens

let semant ast =
    let rec semant' ast sym =
        let semantStmt s (sym: string Set) =
            match s with
            | LetStmt (iden, _) -> Set.add iden sym
            | PrintStmt expr ->
                match expr with
                | Identifier iden when not <| Set.contains iden sym ->
                    raise (SemanticException(sprintf "undefined symbol: '%s'" iden))
                | _ -> sym

        match ast with
        | SingleStmt s -> semantStmt s sym
        | CompoundStmt (s, tl) -> semant' tl (semantStmt s sym)

    let () = ignore (semant' ast Set.empty)
    ast


[<EntryPoint>]
let main argv =
    let ok = 0
    let err = 1

    match Array.length argv with
    | 1 ->
        try
            printfn "%A" (Array.head argv |> lex |> parse |> semant)
            ok
        with
        | LexicalException e ->
            eprintfn "Lexical error at: %s" e
            err
        | SyntaxException e ->
            eprintfn "Syntax error at: %s" e
            err
        | SemanticException e ->
            eprintfn "Semantic error at: %s" e
            err
    | _ ->
        eprintfn "usage: Arithmos '<program>'"
        ok
