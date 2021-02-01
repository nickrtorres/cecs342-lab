>  NB: This problem is adapted from Mr. Neal Terrell's adaptation of exercise
>  8.1 from Scott's _Programming Language Pragmatics (4th edition)_

Consider the following struct:

```c
struct A {
  char a;
  double b;
  char c;
  short *d;
  char e;
  short f;
  char g;
  int h;
};
```


1. How many bytes will a value of type `A` occupy?
2. How many bytes will `A[10]` occupy?
3. Provide an alternate declaration of `A` that reduces the amount of padding
   needed to satisfy the alignment rules.


Assume that:
  * a `char` is 1 byte
  * a `short` is 2 bytes
  * an `int` is 4 bytes
  * a `double` is 8 bytes
  * _all_ pointers are 8 bytes
  * all fields must be aligned to multiples of their size (e.g. a `short` _must_
    be aligned to an address that is a multiple of 2).
  * the compiler _will not_ reorder the fields in the struct; they will be
    allocated as written.
