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
3. How many bytes will `A*[10]` occupy?
4. Provide an alternate declaration of `A` that reduces the amount of padding
   needed to satisfy the alignment rules.


Assume that:
  * a `char` is 1 byte
  * a `short` is 2 bytes
  * an `int` is 4 bytes
  * a `double` is 8 bytes
  * _all_ pointers are 8 bytes
  * all fields must be aligned to multiples of their size (e.g. a `short` _must_
    be aligned to an address that is a multiple of 2).
  * the size of a struct must be a multiple of the size of its largest field
    (e.g. if a struct's largest field is of type `int` then its size must be a
    multiple of 4)
  * the base address that a struct is allocated at is a multiple of its size
    (e.g. if a struct's size is a multiple of 4, then its base address will
    be a multiple of 4).
  * the compiler _will not_ reorder the fields in the struct; they will be
    allocated as written.
  * the compiler _will not_ pack the structure.
