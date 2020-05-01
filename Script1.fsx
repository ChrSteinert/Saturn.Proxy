

let pf (value : PrintfFormat<_,_,_,_,'a>) = sprintf value 



let a = (pf "asd%s%i")
a.Value