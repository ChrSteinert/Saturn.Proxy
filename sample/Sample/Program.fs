open Saturn.Proxy
open Saturn

let r = router {
    proxy "/" "https://bing.com/"
}

[<EntryPoint>]
let main argv =

    let app = application {
        url "http://[::]:8080"
        use_router r
    }

    run app
    
    0 // return an integer exit code
