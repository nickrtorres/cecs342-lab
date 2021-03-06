module ListOps

let suffixes coll =
    let rec suffixes' acc coll =
        match coll with
        | [] -> [] :: acc
        | _ :: tl -> suffixes' (coll :: acc) tl

    suffixes' [] coll |> List.rev
