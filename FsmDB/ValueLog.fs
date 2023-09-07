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

/// Log file of the key-value pairs.
module FsmDB.ValueLog

open System

/// Value Log of the Database.
/// The Value Log stores the values in the database in the order that they were written.
/// New entries are written to the head of the file. To remove values that have been
/// overwritten or deleted, the database runs a garbage collection process that
/// hole-punches the file with `fallocate`. After the file has been hole-punched, the tail
/// is updated to the position of the last known write that is still live.
///
/// The ValueLog entries also hold a copy of the key to speed up the garbage collection
/// procces.
///
/// Creates a new ValueLog or loads an existing one from disk.
/// If the ValueLog file already exists, this function will only open the file without
/// scanning it. This function doesn't check if file has been corrupted.
type ValueLog(initPath: string, initHead: uint32, initTail: uint32) =
    /// The file that the values are written to.
    let mutable __file: IO.FileStream = null

    /// The head of the ValueLog. This is where the next value will be written.
    let mutable __head = initHead

    /// The tail of the ValueLog. This is the position of the oldest write that hasn't been overwritten or deleted.
    let mutable __tail: uint32 = initTail

    do
        if IO.File.Exists(initPath) then
            __file <- new IO.FileStream(initPath, IO.FileMode.Open, IO.FileAccess.Read)
        else
            __file <- new IO.FileStream(initPath, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)

    member this.Head
        with get () = __head
        and set (newHead: uint32) = __head <- newHead

    member this.File = __file
    member this.Tail = __tail

    interface IDisposable with
        /// Since the IO.FileStream initialized does not automatically close,
        /// ValueLog needs to call this function to release related resources.
        member this.Dispose() = __file.Close()

    /// Appends a new key-value pair to the ValueLog.
    /// In the event that the write was unsuccessful, the ValueLog won't shift the head forward.
    /// Therefore, writes can be retried without the risk of polluting the ValueLog or corrupting past entries.
    member public this.Append(key: string, value: string) : uint32 =
        __file.Seek(this.Head |> int64, IO.SeekOrigin.Begin) |> ignore

        let writer = new IO.BinaryWriter(this.File)
        writer.Write(key.Length |> uint64 |> BitConverter.GetBytes)
        writer.Write(value.Length |> uint64 |> BitConverter.GetBytes)
        writer.Write(key |> Text.Encoding.UTF8.GetBytes)
        writer.Write(value |> Text.Encoding.UTF8.GetBytes)

        let pos = this.Head
        __head <- __head + 8u + 8u + ((key.Length + value.Length |> uint32))
        pos

    /// Fetches a value from the ValueLog at a given position.
    member public this.Fetch(loc: int64) : string =
        __file.Seek(loc, IO.SeekOrigin.Begin) |> ignore

        let reader = new IO.BinaryReader(this.File)
        let keyLength = reader.ReadBytes(8) |> BitConverter.ToUInt64 |> int
        let valueLength = reader.ReadBytes(8) |> BitConverter.ToUInt64 |> int

        // Ignore the key
        reader.ReadBytes(keyLength) |> ignore
        reader.ReadBytes(valueLength) |> Text.Encoding.UTF8.GetString

    /// Syncs the ValueLog to the disk.
    /// This function forcefully flushes the changes to the ValueLog to disk. Use this function
    /// sparingly as is will heavily reduce the write throughput of the ValueLog.
    member public this.Sync() = __file.Flush(true)
