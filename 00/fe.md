## An overview of (front-end) compiler design

_Classical_ compilers split into two phases: front and back end [1].
Furthermore, each unit within a given phase provides an interface -- that is, a
contract -- of what clients can expect on the other side. A typical front-end
may look something like this [2]:

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
                             | Token stream (e.g. std::vector<Token>)
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

NB
[1]: Modern compilers are a bit different: https://youtu.be/wSdV1M7n4gQ
[2]: See for yourself! Rust's original compiler was written in OCaml and
followed this form pretty closely: https://github.com/rust-lang/rust/tree/ef75860a0a72f79f97216f8aaa5b388d98da6480/src/boot/fe
