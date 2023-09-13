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

module FsmDB.Tests.ValueLog

open FsmDB.ValueLog
open NUnit.Framework
open System

[<Test>]
let ``Initialize ValueLog`` () =
    let log = new ValueLog("value_log.data", 0u, 128u)
    Assert.AreEqual(0u, log.Head)
    Assert.AreEqual(128u, log.Tail)

    IO.File.Delete("value_log.data")

[<Test>]
let ``Append to ValueLog`` () =
    let log = new ValueLog("value_log.data", 0u, 128u)
    let key = "apple"
    let value = "Apple Pie"
    let pos = log.Append(key, value)
    log.Sync()
    Assert.AreEqual(0, pos)

    use file =
        new IO.BinaryReader(new IO.FileStream("value_log.data", IO.FileMode.Open, IO.FileAccess.Read))

    let keyLength = file.ReadBytes(8) |> BitConverter.ToUInt64
    let valueLength = file.ReadBytes(8) |> BitConverter.ToUInt64

    Assert.AreEqual(keyLength, key.Length)
    Assert.AreEqual(valueLength, value.Length)
    Assert.AreEqual(key, file.ReadBytes(keyLength |> int) |> Text.Encoding.UTF8.GetString)
    Assert.AreEqual(value, file.ReadBytes(valueLength |> int) |> Text.Encoding.UTF8.GetString)

    IO.File.Delete("value_log.data")
