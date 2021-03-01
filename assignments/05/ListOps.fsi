module ListOps

// The following problem comes from Chris Okasaki's book, "Purely Functional
// Data Structures".
//
// Write a function, suffixes, that breaks a list into all of its suffixes in
// decreasing order. Your implementation must be recursive. You are allowed to
// use the `reverse` function that was shown in class.
//
// Examples:
// input: suffixes [1; 2; 3; 4]
// output: [[1; 2; 3; 4]; [2; 3; 4]; [3; 4]; [4]; []]
//
// input: suffixes [1; 2; 3]
// output: [[1; 2; 3]; [2; 3]; [3]; []]
val suffixes: 'a list -> 'a list list
