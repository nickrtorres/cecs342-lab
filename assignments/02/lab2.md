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

    A value of type `A` will occupy 48 bytes. A depiction of `A`'s layout in
    memory is given below.

    ```
    byte offset |     struct field
    (decimal)   |     (or padding)
    ------------+-------------------------

         0      +-----------------------+
                |           a           |
         1      +-----------------------+
                |///////////////////////|
         2      +-----------------------+
                |///////////////////////|
         3      +-----------------------+
                |///////////////////////|
         4      +-----------------------+
                |///////////////////////|
         5      +-----------------------+
                |///////////////////////|
         6      +-----------------------+
                |///////////////////////|
         7      +-----------------------+
                |///////////////////////|
         8      +-----------------------+
         .      |                       |
         .      +           b           +
         .      |                       |
        16      +-----------------------+
                |           c           |
        17      +-----------------------+
                |///////////////////////|
        18      +-----------------------+
                |///////////////////////|
        19      +-----------------------+
                |///////////////////////|
        20      +-----------------------+
                |///////////////////////|
        21      +-----------------------+
                |///////////////////////|
        22      +-----------------------+
                |///////////////////////|
        23      +-----------------------+
                |///////////////////////|
        24      +-----------------------+
         .      |                       |
         .      +           d           +
         .      |                       |
        32      +-----------------------+
                |           e           |
        33      +-----------------------+
                |///////////////////////|
        34      +-----------------------+
                |                       |
        35      +           f           +
                |                       |
        36      +-----------------------+
                |           g           |
        37      +-----------------------+
                |///////////////////////|
        39      +-----------------------+
                |///////////////////////|
        40      +-----------------------+
         .      |                       |
         .      +           h           +
         .      |                       |
        44      +-----------------------+
                |///////////////////////|
        45      +-----------------------+
                |///////////////////////|
        46      +-----------------------+
                |///////////////////////|
        47      +-----------------------+
                |///////////////////////|
        48      +-----------------------+
     ```

2. How many bytes will `A[10]` occupy?

    `A[10]` will occupy `sizeof(struct A)` * 10 =  48 * 10 = 480 bytes.

3. How many bytes will `A*[10]` occupy?

    `A*[10]` will occupy `sizeof(struct A *)` * 10 =  8 * 10 = 80 bytes.

4. Provide an alternate declaration of `A` that reduces the amount of padding
   needed to satisfy the alignment rules.

    The following struct declaration occupies 32 bytes (30% smaller!).

    ```c
    struct A {
      double b;
      short *d;
      int h;
      short f;
      char a;
      char c;
      char e;
      char g;
    };
    ```

    Its layout in memory is depicted below.

    ```
    byte offset |     struct field
    (decimal)   |     (or padding)
    ------------+-------------------------

         0      +-----------------------+
         .      |                       |
         .      +           b           +
         .      |                       |
         8      +-----------------------+
         .      |                       |
         .      +           d           +
         .      |                       |
        16      +-----------------------+
         .      |                       |
         .      +           h           +
         .      |                       |
        20      +-----------------------+
                |                       |
        21      +           f           +
                |                       |
        22      +-----------------------+
                |           a           |
        23      +-----------------------+
                |           c           |
        24      +-----------------------+
                |           e           |
        25      +-----------------------+
                |           g           |
        26      +-----------------------+
                |///////////////////////|
        27      +-----------------------+
                |///////////////////////|
        28      +-----------------------+
                |///////////////////////|
        29      +-----------------------+
                |///////////////////////|
        30      +-----------------------+
                |///////////////////////|
        31      +-----------------------+
                |///////////////////////|
        32      +-----------------------+
     ```

## Assume that:
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
