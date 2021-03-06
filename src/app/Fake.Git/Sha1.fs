﻿[<AutoOpen>]
module Fake.Git.SHA1

open System.Security.Cryptography
open System.Text

/// Calculates the SHA1 for a given string
let calcSHA1 (text:string) =
    Encoding.Default.GetBytes text
      |> (new SHA1CryptoServiceProvider()).ComputeHash
      |> Array.fold (fun acc e -> 
           let t = System.Convert.ToString(e, 16)
           if t.Length = 1 then acc + "0" + t else acc + t) 
           ""
/// Calculates the SHA1 like git
let calcGitSHA1 (text:string) =
    let s = text.Replace("\r\n","\n")
    sprintf "blob %d%c%s" s.Length (char 0) s
      |> calcSHA1

/// shows the SHA1 calculated by git
let showObjectHash repositoryDir fileName =
    let _,msg,_ = runGitCommand repositoryDir (sprintf "hash-object %s" fileName)
    msg |> Seq.head