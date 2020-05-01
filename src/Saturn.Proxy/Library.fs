namespace Saturn.Proxy

open System
open System.IO
open System.Net
open System.Net.Http
open System.Threading

open Saturn
open Giraffe
open Microsoft.Extensions.Primitives

module Saturn =

    type Saturn.Router.RouterBuilder with
        
        [<CustomOperation("proxy")>]
        member this.Proxy(state, path : string, target : string) = 
            this.Forward(state, path, (fun next ctx -> 
                let reqTarget = (Uri(Uri(target), ctx.Request.Path.Value.Substring(path.Length) + ctx.Request.QueryString.Value))
                let cts = new CancellationTokenSource(20000)

                use handler = new HttpClientHandler()
                use invoker = new HttpMessageInvoker(handler)
                use m = new HttpRequestMessage(HttpMethod.Get, reqTarget)
                ctx.Request.Headers
                |> Seq.filter (fun c -> c.Key <> "Host")
                |> Seq.filter (fun c -> c.Key <> "Accept-Encoding")
                |> Seq.iter (fun c -> m.Headers.TryAddWithoutValidation(c.Key, c.Value.ToArray ()) |> ignore)
                try
                    let r = invoker.SendAsync(m, cts.Token).Result
                    r.Headers
                    |> Seq.filter (fun c -> c.Key <> "Transfer-Encoding")
                    |> Seq.iter (fun c -> ctx.Response.Headers.Add(c.Key, StringValues(c.Value |> Seq.toArray)))
                    if r.Content.Headers.ContentType <> null then
                        ctx.Response.ContentType <- r.Content.Headers.ContentType.ToString ()
                    ctx.Response.StatusCode <- r.StatusCode |> int
                    r.Content.ReadAsByteArrayAsync().Result |> setBody
                with
                | e -> 
                    printfn "%O" e
                    setStatusCode 503
                |> fun c -> c next ctx
            ))

        [<CustomOperation("proxyf")>]
        member this.Proxyf(state : RouterState, path : PrintfFormat<_, _, _, _, 'a>, targetBuilder : 'a -> string) = 
            let a = PrintfFormat<_,_,_,_,_> path.Value
            state
            
            


    //let a = router {
    //    get "" (failwith "")
    //    proxy "/" "https://example.com/"
    //} 
