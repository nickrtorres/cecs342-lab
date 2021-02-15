module ListOps

type Account =
    | Balance of int
    | Overdrawn of int
    | Empty

// For the functions described below, first give a comment that expresses the
// type of the function, then provide an implementation that matches the
// description. (Note: make sure you understand the type of the function if your
// editor provides you with the type automatically). All of the functions below
// take lists. Make sure to note when the list can be generic.
//
// The implementation *must* be recursive.
//
// Descriptions below were provided by Mr. Neal Terrell.

// count x coll
//
// count the number of values equal to x in coll.


// countEvens coll
//
// count the number of even integers in coll.


// lastElement coll
//
// return the last element in the list


// maxOverdrawn coll
//
// given a list of Accounts, return the largest Overdrawn amount, or 0 if none
// are overdrawn
