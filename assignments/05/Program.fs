open ListOps

exception CheckFailedException of string

let check expected actual steps =
    if expected <> actual then
        let baseMsg =
            sprintf "Check failed. Expected: '%A' got: '%A'. Performed the following steps:\n" expected actual


        raise (CheckFailedException $"{baseMsg}\t{steps}")

[<EntryPoint>]
let main argv =
    try 
        check [[]] (suffixes []) "suffixes []"
        check [[1; 2; 3]; [2; 3]; [3]; []] (suffixes [1; 2; 3]) "suffixes [1; 2; 3]"
        check [[1; 2; 3; 4]; [2; 3; 4]; [3; 4]; [4]; []] (suffixes [1; 2; 3; 4]) "suffixes [1; 2; 3; 4]"
        0
    with CheckFailedException e ->
        eprintfn "%s" e
        1

