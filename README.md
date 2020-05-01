# Saturn.Proxy

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Saturn.Proxy?style=flat-square&logo=nuget)


A super simple, naive (not really serious) reverse proxy implementation for Saturn. 
It provides a new custom operation `proxy` for Saturn's `router` Route Builder.

It can be used to forward requets when developing websites (for instance with SAFE) or just generic proxy work.

## Usage

_See the sample_

On any router, just add 
```fs
let r = router {
    …
    proxy "/" "https://<my new endpoint>/"
}
```
