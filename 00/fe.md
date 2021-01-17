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

This overview will focus on the phases of the compiler work to understand (and
detect errors in) the *syntax* and *semantics* of a compiler. These phases are:
*lexical analysis*, *parsing*, and *semantic analysis* -- together they make up
the front end of a typical compiler.

## Lexical Analysis



## References
[2]: See for yourself! Rust's original compiler was written in OCaml and
followed this form pretty closely: https://github.com/rust-lang/rust/tree/ef75860a0a72f79f97216f8aaa5b388d98da6480/src/boot/fe
