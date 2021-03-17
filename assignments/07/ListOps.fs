module ListOps

type Account =
    | Overdrawn of int
    | Balance of int
    | Empty

type Customer = { Name: string; Account: Account }

let withdraw amount acc =
    match acc with
    | Empty -> Overdrawn amount
    | Overdrawn n -> Overdrawn(amount + n)
    | Balance n when n - amount > 0 -> Balance(n - amount)
    | Balance n when n - amount < 0 -> Overdrawn(abs (n - amount))
    | Balance _ -> Empty

let deposit amount acc =
    match acc with
    | Empty -> Balance amount
    | Balance n -> Balance(amount + n)
    | Overdrawn n when n - amount > 0 -> Overdrawn(n - amount)
    | Overdrawn n when n - amount < 0 -> Balance(abs (n - amount))
    | Overdrawn _ -> Empty

// fun acc elem -> <combine the current element with the acc somehow>
//
//     using empty string as starting point
//     ------------------------------------
//     expected value = "this is text"
//     coll = ["this"; "is"; "text"]
//     delim = " "
//
//     acc         apply function
//     --------------------------
//     ""          "" + " " + "this"         -> " this"
//     " this"     " this" + " " + "is"      -> " this is"
//     " this is"  " this is" + " " + "text" -> " this is text"[1:]
//
//     OR
//
//     using hd as the starting point
//     ------------------------------
//     expected value = "this is text"
//     expected value = "this is text"
//     coll = ["this"; "is"; "text"]
//     delim = " "
//
//     acc         elem
//     ----------------
//     "this"      "this" + " " + "is"      -> "this is"
//     "this is"   "this is" + " " + "text" -> "this is text"
let join delim coll =
    match coll with
    | [] -> ""
    | hd :: tl -> List.fold (fun acc elem -> acc + delim + elem) hd tl


// The keys to successfully solving this problem:
// - use the types
// - use 'programming by wishful thinking'
// - keep it simple
let simplifyBank customers names =
    // assumption: every customer in this list has the same name
    let reduceCustomer customers =
        let combineCustomers acc elem =
            match elem.Account with
            | Balance b ->
                { acc with
                      Account = deposit b acc.Account }
            | Overdrawn o ->
                { acc with
                      Account = withdraw o acc.Account }
            | Empty -> acc

        List.reduce combineCustomers customers

    // what must the type of simplifyCustomer be?
    // Customer list -> string -> Customer list
    let simplifyCustomer acc name =
        (List.filter (fun c -> c.Name = name) customers
         |> reduceCustomer)
        :: acc

    List.fold simplifyCustomer [] names |> List.rev
