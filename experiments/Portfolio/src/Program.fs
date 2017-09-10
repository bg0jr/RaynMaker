module Portfolio.Main

open Suave
open Suave.Successful
open Suave.Operators
open Suave.Filters
open System.Net
open System.Threading
open System
open System.Diagnostics


[<EntryPoint>]
let main argv = 
    printfn "Starting ..."

    let home () = 
        "hello"

    let app : WebPart =
        choose [ 
            path "/" >=> OK (home ()) 
        ]

    let local = HttpBinding.create HTTP IPAddress.Loopback 2525us
                
    let cts = new CancellationTokenSource()
    let config = { defaultConfig with bindings = [ local ]
                                      cancellationToken = cts.Token }

    let listening, server = startWebServerAsync config app
    
    Async.Start(server, cts.Token)

    Process.Start("http://localhost:2525") |> ignore
    
    
    Console.ReadKey true |> ignore
    
    cts.Cancel()


    0