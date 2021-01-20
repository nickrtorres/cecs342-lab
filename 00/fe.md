## An overview of (front end) compiler design

---

NB: This document provides an overview to chapter 1.6 from *Programming
Language Pragmatics*. Helpful links are inlined throughout. If you see any
errors, feel free to open an issue or PR to report or fix them.

---

This overview will focus on the phases of the compiler that work to understand
(and detect errors in) the *syntax* and *semantics* of a source program. These
phases are: *lexical*, *syntax*, and *semantic* analysis -- together they make
up the front end of a typical compiler. A depiction of these phases is provided
below.

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

## Lexical Analysis

Lexical analysis breaks a source program into tokens that are used by the
parser to build subsequent data structures.

A token will generally have
1. A type or tag
2. A line number (from the original source text)
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
an error that indicates that there is a lexical error in the program -- namely,
the existence of the `~` character in the program.

## Syntax Analysis

After breaking the source text into a stream of tokens, the lexer passes
control to the parser. The parser is responsible for analyzing the syntax of a
program. To verify that the program is well formed, a parser uses a
context-free grammar.  [Context-free grammars][CFG] consist of a single start
symbol and a finite number of terminal and nonterminal symbols.

Context-free grammars are often expressed in [Backus-Naur form][BNF] (BNF). BNF
uses angle brackets for nonterminals and the `::=` symbol to denote a
production rule. Everything else (i.e. an element not in angle brackets) is
considered a terminal symbol.

An example context-free grammar is given below for a simple made up language,
Arithmos.
```
<program>         ::= <stmt_list>
<stmt_list>       ::= <stmt>
<stmt_list>       ::= <stmt> ; <stmt_list>

<stmt>            ::= <input_stmt>
<stmt>            ::= <let_stmt>
<stmt>            ::= <print_stmt>

<input_stmt>      ::= input identifier
<print_stmt>      ::= print <expr>
<let_stmt>        ::= let identifier = <expr>

<expr>            ::= identifier
<expr>            ::= nil
<expr>            ::= number
```

In this example, `<program>` is the start symbol. The nonterminals are `{
<program>, <stmt_list>, <stmt>, <input_stmt>, <print_stmt>, <let_stmt>, <expr>
}`. The terminals are `{ ';', input, print, let, identifier, nil, number }`.

### Derivations and errors

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
*[derivation]*.  This derivation means that `let x = 42` is a valid statement
in Arithmos.

Alternatively, consider the statement: `var x = 42`. Expanding this statement
with the same grammar results in the following derivation.
```
<program>     ::=  <stmt_list>
              ::=  <stmt>
              ::=  ERROR: no matching rule beginning with `var ...`
```

Failure to expand the statement `var x = 42` represents a *syntax error* --
that is, there aren't *any* rules in our grammar that match this statement.

Consider another statement: `let x = 42 print x`. The attempted derivation looks like
this.
```
<program>     ::=  <stmt_list>
              ::=  <stmt> ; <stmt_list>
              ::=  <let_stmt> ; <stmt_list>
              ::= let x = <expr> ; <stmt_list>
              ::= let x = 42 ERROR: expected semicolon
```

The grammar rules only allow compound statements if they are separated by a
semicolon. It is a syntax error to omit the semicolon.

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

## Semantic Analysis

After parsing the compiler starts semantic analysis to understand and verify
the meaning of a program. For this overview we'll focus on two categories of
errors that can occur during semantic analysis: static and dynamic.

### Static errors

Consider the program: `let x = 42 ; print y`

Since `y` was never bound to a value, attempting to print `y` is a *semantic
error*. More specifically, it is a *static* semantic error -- there is enough
information known at compile time halt compilation and inform the programmer
of an error.

But how does the compiler know that `y` wasn't defined? A compiler will build a
symbol table by traversing the intermediate representation produced by the
parser.

```
                            CompoundStmt
                                  +
                                  |
                                 / \
  LetStmt(Identifier(x), Num(42))   StmtList
                                       +
                                       |
                                  SingleStmt
                                       +
                                       |
                            PrintStmt(Indentifier(y))

```

A compiler can build a symbol table by traversing this tree-like structure and
performing an action on each node depending on the variant:
  1. If the node is a `LetStmt`: add the identifier to the symbol table.
  2. If the node is a `PrintStmt`: check for the identifier in the symbol
     table. Raise a `SemanticException` if it doesn't exist in the symbol
     table.

In a  language like Java, semantic analysis might also involve type checking.
Consider the snippet below:

```java
int x = 42;
String s = x;
```

The Java compiler will produces the following error since a `String` cannot be
bound to an `int`.

```
error: incompatible types: int cannot be converted to String
    String s = x;
```

Static semantic analysis can become quite sophisticated. For example, the Rust
programming language statically enforces rules on mutation and lifetimes.

### Dynamic errors

Dynamic errors manifest in areas that are only known at runtime. These errors
are specified by the language designer.

Language designers decide what constitutes an error in language. If the
language designer decides to enforce invariants at runtime, then the resulting
program will have more machine instructions to check those invariants (and
potentially throw an exception or crash if they no longer hold).

C pushes the safety responsibility onto the programmer and does not check
whether a pointer is null before attempting to dereference it -- attempting to
dereference null is considered [undefined behavior]. Conversely, Java will
raise a `NullPointerException` when a programmer attempts to deference a null
reference.

## A summary of errors

So, to wrap it up, we talked about three categories of errors: lexical, syntax,
and semantic. In most compilers, they are found in that order, since phases of
compilation occur one after the other.

Lexical errors occur when a token is malformed or does not exist in the language.
Some examples include:
  - Identifiers that don't conform to the rules of the language (e.g. If a
    language requires identifiers to begin with a lowercase letter, then $foo
    is a malformed identifier).
  - Invalid symbols (e.g. Unicode symbols in a language that only supports the
    ascii character set).

Syntax errors occur when a valid production rule fails to match for the token
stream produced by the lexer.  Some examples include:
  - `int x y = 10;` in C -- Adjacent identifiers are only allowed if they are
    separated by a comma; not whitespace.
  - `int 10 = 10` in C -- 10 is not a valid identifier, but is a valid token in
    the language.

Semantic errors occur when the meaning of a program doesn't make sense. Some
semantic errors can be checked statically, others require additional machine
instructions at runtime. Some examples include:
  - `int x = "foo"` is a static semantic error in Java.
  - `int[] xs = {42}; xs[1000] = 42` is a dynamic semantic error in Java (the
    Java runtime will throw `java.lang.ArrayIndexOutOfBoundsException`).
  - `void f(void) {} int main() { f(42); return 0 }` in C -- `f` is a void
    function that takes 0 arguments.  Calling `f` with 1 argument is a static
    semantic error.

## Example

There is an example [lexer], [parser], and [semantic analyzer][semant] for
Arithmos -- the language described above -- in the [example/] directory.  The
implementation is about 150 lines of F#.

Some possible exercises (if you're interested):
- The lexer splits on whitespace. This means that semicolons must be surrounded
  by whitespace ` ; `. Try updating the lexer to support statements without a
  space between the semicolon (e.g. `let x = 42; print x` instead of `let x =
  42 ; print x`).

## Additional resources and other cool stuff

Example grammars:
- Lua: http://lua-users.org/wiki/LuaGrammar
- Python: https://docs.python.org/3/reference/grammar.html
- SML: https://people.mpi-sws.org/~rossberg/sml.html
- C# (along with quite a bit of explanation!):
  https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/lexical-structure#syntactic-grammar


See for yourself! Rust's original compiler was written in OCaml and follows a
similar form described above.
  https://github.com/rust-lang/rust/tree/ef75860a0a72f79f97216f8aaa5b388d98da6480/src/boot/fe


<!-- Links -->
[BNF]: https://en.wikipedia.org/wiki/Backus%E2%80%93Naur_form
[CFG]: https://en.wikipedia.org/wiki/Context-free_grammar
[algebraic data types]: https://en.wikipedia.org/wiki/Algebraic_data_type
[derivation]: https://en.wikipedia.org/wiki/Context-free_grammar#Derivations_and_syntax_trees
[example/]: https://github.com/nickrtorres/cecs342-lab/tree/master/00/example
[lex]: https://en.wikipedia.org/wiki/Lex_(software)
[lexeme]: https://en.wikipedia.org/wiki/Lexical_analysis#Lexeme
[lexer]: https://github.com/nickrtorres/cecs342-lab/blob/master/00/example/Program.fs#L25
[parser]: https://github.com/nickrtorres/cecs342-lab/blob/master/00/example/Program.fs#L62
[semant]: https://github.com/nickrtorres/cecs342-lab/blob/master/00/example/Program.fs#L115
[undefined behavior]: https://en.wikipedia.org/wiki/Undefined_behavior
