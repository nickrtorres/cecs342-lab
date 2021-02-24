type Account =
    | Balance of int
    | Overdrawn of int
    | Empty

// val maxOverdrawn: Account list -> int
let maxOverdrawn accounts =
    let rec maxOverdrawn' accounts acc =
        match accounts with
        | [] -> acc
        | Overdrawn b :: tl when b > acc -> maxOverdrawn' tl b
        | Overdrawn b :: tl -> maxOverdrawn' tl acc
        | _ :: tl -> maxOverdrawn' tl acc

    maxOverdrawn' accounts 0

assert (200 = maxOverdrawn [ Balance 100
                             Overdrawn 100
                             Overdrawn 50
                             Overdrawn 200 ])

// val indexOf: 'a -> 'a list -> int
let indexOf x coll =
    let rec indexOf' coll acc =
        match coll with
        | [] -> failwith "index out of bounds!"
        | hd :: _ when hd = x -> acc
        | _ :: tl -> indexOf' tl (acc + 1)

    indexOf' coll 0

assert (1 = indexOf 4 [ 2; 4; 6; 8 ])

// val valueAt: int -> 'a list -> 'a
// This is left as an exercise for the reader

// val countEvens: int list -> int
// This is left as an exercise for the reader

// val filterEvens:  int list -> int list
let filterEvens coll =
    let rec filterEvens' coll acc =
        match coll with
        | hd :: tl when hd % 2 = 0 -> filterEvens' tl (hd :: acc)
        | _ :: tl -> filterEvens' tl acc
        | [] -> acc

    filterEvens' coll []

// Check you understanding: why [4; 2] and not [2; 4]?
assert ([ 4; 2 ] = filterEvens [ 1; 2; 3; 4; 5 ])

[<EntryPoint>]
let main argv = 0
