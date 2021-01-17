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

In a language with [algebraic data types] (i.e. F#, Rust, Haskell, etc.), the
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

## References
[2]: See for yourself! Rust's original compiler was written in OCaml and
followed this form pretty closely: https://github.com/rust-lang/rust/tree/ef75860a0a72f79f97216f8aaa5b388d98da6480/src/boot/fe


[lex]: https://en.wikipedia.org/wiki/Lex_(software)
[lexeme]: https://en.wikipedia.org/wiki/Lexical_analysis#Lexeme
[algebraic data types]: https://en.wikipedia.org/wiki/Algebraic_data_type
