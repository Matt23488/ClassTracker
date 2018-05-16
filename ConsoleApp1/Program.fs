// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =

    // Notice the type of "add" from Intellisense
    let add a b c =
        a + b + c

    let addFive = add 5
    let addNine = addFive 4

    printfn "add 5 4 10 = %d" (add 5 4 10)
    printfn "addNine 10 = %d" (addNine 10)

    0
