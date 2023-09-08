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


/// Write-Ahead Log for the keys and the value positions.
module FsmDB.WAL

open System
open Memtbl

/// Write-Ahead Log(WAL) of the Database.
/// The WAL holds a running log of the operations that were applied to the Memtbl. When
/// the database restarts, the WAL is replayed to recover the Memtbl.
type WAL(initPath: string) =
    let __file: IO.FileStream =
        //  If a WAL already exists at this path, the file will not be overwritten.
        new IO.FileStream(initPath, IO.FileMode.Open, IO.FileAccess.ReadWrite)

    let __path: string = initPath

    do __file.Seek(0, IO.SeekOrigin.End) |> ignore

    member public this.File = __file
    member public this.Path = __path

    interface IDisposable with
        member this.Dispose() = __file.Close()

    /// Replays the WAL from the start and recreates the MemTable.
    /// This is the main recovery function for the WAL for when the Database restarts.
    member public this.Load(memtbl: Memtbl) : unit =
        __file.Seek(0L, IO.SeekOrigin.Begin) |> ignore

        let reader = new IO.BinaryReader(__file)

        while true do
            if -1 = reader.PeekChar() then
                ()
            else
                let walKeyLength = reader.ReadBytes(8) |> BitConverter.ToUInt64
                let walValueLoc = reader.ReadBytes(8) |> BitConverter.ToInt64
                let walKey = reader.ReadBytes(walKeyLength |> int) |> Text.Encoding.UTF8.GetString

                if -1L = walValueLoc then
                    memtbl.Remove({ Key = walKey; ValueLoc = walValueLoc }) |> ignore
                else
                    memtbl.Add({ Key = walKey; ValueLoc = walValueLoc })

    /// Appends a new Memtbl operation to the WAL
    /// For sets, the `ValueLoc` should be the position in the ValueLog that the value entry
    /// was written to. For deletes, `ValueLoc` should be -1 to indicate a tombstone.
    member public this.Add(record: Record) : unit =
        let writer = new IO.BinaryWriter(__file)
        writer.Write(record.Key.Length |> uint64 |> BitConverter.GetBytes)
        writer.Write(record.ValueLoc |> int64 |> BitConverter.GetBytes)
        writer.Write(record.Key |> Text.Encoding.UTF8.GetBytes)

    /// Syncs the WAl to the disk.
    ///  This function forcefully flushes the changes to the WAL to disk. Use this function
    /// sparingly as is will heavily reduce the write throughput of the WAL.
    member public this.Sync() = __file.Flush(true)
