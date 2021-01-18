## An overview of (front-end) compiler design

NB: This overview roughly correlates to chapter 1.6 from *Programming Language
Pragmatics*. Ancillary references are listed in the [references section] at the
bottom of this file.

---

Compilers work in a number of phases (often called passes) that transform a
source text program into machine instructions that can be executed by a target
CPU. Each phase is an abstraction that provides an interface (or contract) to
the next phase. An overview of these phases is depicted below.

```
                             | Source program (e.g. main.cpp)
                             |
                             V
                +--------------------------+
                |                          |
                |          Lexer           |
                |                          |
                +--------------------------+
                             |
                             | Token stream
                             V
                +--------------------------+
                |                          |
                |          Parser          |
                |                          |
                +--------------------------+
                             |
                             | Parse tree
                             V
                +--------------------------+
                |                          |
                |    Semantic Analysis     |
                |                          |
                +--------------------------+
                             |
                             | Abstract syntax tree (IR)
                             V
              +------------------------------+
              |                              |
              |                              |
              |                              |
              |                              |
              |          Back end ...        |
              |                              |
              |                              |
              |                              |
              |                              |
              +------------------------------+

```

This overview will focus on the phases of the compiler that work to understand
(and detect errors in) the *syntax* and *semantics* of a compiler. These phases
are: *lexical analysis*, *parsing*, and *semantic analysis* -- together they
make up the front end of a typical compiler.

## Lexical Analysis

Lexical analysis breaks a source program into its smallest, *meaningful* units.
It generates a stream of tokens that the parser can pull from in the subsequent
phases of the compiler.

A token will generally have
1. A type or tag
2. A line number (from the original source text. This may be omitted when using
   a lexical analysis generator like [lex] if the generator provides an API to
   obtain this information without encoding it into the type.)
3. A [lexeme] -- that is, the underlying fragment from the source text that
   corresponds to the token.

In C++ a token can take the form of a class or struct.
```cpp
struct Token {
  enum class Type {
    LParen,
    RParen,
    Identifier,
    If,
    Else,
    // ...
  };

  Type mType;
  int mLine;
  std::string mLexeme;
};
```

In a language with [algebraic data types] (e.g. F#, Rust, Haskell, etc.), the
tag can be encoded directly as one of the variants.
```fsharp
type Token =
  | LParen of int
  | RParen of int
  | Identifier of string * int
  | If of int
  | Else of int
  // ...
```

### Errors

A lexer will uncover *lexical errors* in a program. Lexical errors manifest in
*lexemes* that do not exist in the source language. Consider the following
fragment of source code.

```
let ~ = 100 ;;
```

If the character `~` does not exist in our language, then the lexer can return
an error that indicates that there is a lexical program with the program --
namely, the existence of the `~` character in the program.

## Syntax Analysis

After breaking the source text into a stream of tokens, the lexer passes
control to the parser. The parser is responsible for analyzing the syntax of a
program. 

Context-free grammars consist of a single start symbol and a finite number of
terminal and nonterminal symbols. Nonterminal symbols expand into other
nonterminal or terminal symbols through *production rules*. The set of terminal
symbols in a context free grammar make up the alphabet of the language [cite].

Context-free grammars are often expressed in Backus-Naur form (BNF). BNF uses
angle brackets for nonterminals and the metasyntactic symbol `::=` to denote a
production rule. Everything else (i.e. an element not in angle brackets) is
considered a terminal symbol.

An example context-free grammar is given below for the made up language,
Arithmos. Arithmos only supports binding statements of the form `let foo = bar`
and printing statements of the form `print foo`.

```
<program>         ::= <stmt_list>
<stmt_list>       ::= <stmt>
<stmt_list>       ::= <stmt> ; <stmt_list>

<stmt>            ::= <print_stmt>
<stmt>            ::= <let_stmt>

<print_stmt>      ::= print <expr>

<let_stmt>        ::= let identifier = <expr>

<expr>            ::= number
<expr>            ::= identifier

```

In this example, <program> is the start symbol. The nonterminals are
{ <program>, <stmt_list>, <stmt>, <print_stmt>, <let_stmt>, <expr> }. The
terminals are { ';', print, let, identifier, number }.

Consider the following Arithmos statement: `let x = 42`. Expanding this
statement with the grammar above looks like this.

```
<program>     ::=  <stmt_list>
              ::=  <stmt>
              ::=  <let_stmt>
              ::=  let x = <expr>
              ::=  let x = 42
```
The expansion of a statement from a context-free grammar is called a
*derivation*.  From this derivation, we can conclude that the statement `let x
= 42` is valid in the Arithmos language.

Alternatively, consider the statement: `var x = 42`. Expanding this statement
with the same grammar results in the following derivation.

```
<program>     ::=  <stmt_list>
              ::=  <stmt>
              ::=  ERROR: no matching rule beginning with `var ...`
```

Failure to expand the statement `var x = 42` represents a *syntax error* --
there aren't *any* rules in our grammar that match this statement.

Finally, consider the statement `let x = y`. Expanding this statement yields
the following derivation.
```
<program>     ::=  <stmt_list>
              ::=  <stmt>
              ::=  <let_stmt>
              ::=  let x = <expr>
              ::=  let x = y
```

Since `y` is a valid identifier, this statement yields a valid derivation.
However, it may be the case that `y` was never defined in an earlier statement
in the program. This is not something that a context-free grammar can identify.
This requires a new phase: semantic analysis.

## References
[2]: See for yourself! Rust's original compiler was written in OCaml and
followed this form pretty closely: https://github.com/rust-lang/rust/tree/ef75860a0a72f79f97216f8aaa5b388d98da6480/src/boot/fe


[lex]: https://en.wikipedia.org/wiki/Lex_(software)
[lexeme]: https://en.wikipedia.org/wiki/Lexical_analysis#Lexeme
[algebraic data types]: https://en.wikipedia.org/wiki/Algebraic_data_type
