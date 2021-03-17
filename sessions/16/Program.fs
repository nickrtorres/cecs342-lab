// Want the ability for a type to either have *a* value or no value
type 'a MyOption =
    | None
    | Some of 'a

// return true if opt is Some ('a)
//        false otherwise
let isSome opt =
    match opt with
    | Some _ -> true
    | None -> false

let isNone opt = not (isSome opt)

// return the underlying value held by the Some variant. fail if the variant is
// None
let getValue opt =
    match opt with
    | Some v -> v
    | None -> failwith "calling getValue on None variant"

// return either:
//   - the indexOf value
//   - the option type we defined above?
let indexOf value coll =
    let rec indexOf' coll i =
        match coll with
        | [] -> None
        | h :: _ when h = value -> Some i
        | _ :: tl -> indexOf' tl (i + 1)

    indexOf' coll 0

assert (isSome (indexOf 2 [ 1; 2; 3; 4; 5 ]))
assert (getValue (indexOf 2 [ 1; 2; 3; 4; 5 ]) = 1)
assert (isNone (indexOf 42 [ 1; 2; 3; 4; 5 ]))

[<EntryPoint>]
let main _ = 0
