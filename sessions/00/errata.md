- I mentioned that JavaScript does not consider it a semantic error to use an
  identifier that hasn't been bound to a value. This is not true. The
  [ECMAScript] spec [states] that conforming implementations will raise a
  `ReferenceException` when attempting to get the value an identifier that is
  not bound to a value.

- The reference implementation for Arithmos did not correctly check if the
  source identifier was defined when creating a new binding with a `let`
  statement (i.e. `let x = y` did not raise a SemanticException even though it
  should have). This has been [corrected].


<!-- Links -->
[corrected]: https://github.com/nickrtorres/cecs342-lab/commit/4205b9443d26423664e39e0205251565b5ac851a
[states]: https://262.ecma-international.org/11.0/#sec-declarative-environment-records-getbindingvalue-n-s
[ECMAScript]: https://en.wikipedia.org/wiki/ECMAScript
