(*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2022 Muqiu Han
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *)

module FsmDB.Record

open System
open SkipList

type Record(key: string, valueLoc: int64) =

    member public this.Key = key |> Text.Encoding.UTF8.GetBytes
    member public this.KeyLength = this.Key.Length |> uint64
    member public this.ValueLoc = valueLoc

    member this.Compare(y: string) : int =
        Array.compareWith (fun (b1: byte) b2 -> b1.CompareTo(b2)) this.Key (y |> Text.Encoding.UTF8.GetBytes)

    interface IComparable<Record> with
        member this.CompareTo(y: Record) : int =
            Array.compareWith (fun (b1: byte) b2 -> b1.CompareTo(b2)) this.Key y.Key

    /// +----------------------------------------------------------------------+
    /// | this.Key.Length (uint64) | this.ValueLoc (int64) | this.Key (string) |
    /// +----------------------------------------------------------------------+
    member public this.Size: int = 8 + 8 + this.Key.Length
