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


/// Non-volatile storage of the keys in the Database.
module FsmDB.SSTable

open System
open Memtbl

/// Single Record in a SSTable.
/// Each SSTableRecord holds the key and the position of the record in the ValueLog.
type SSTableRecord =
    {
        /// The key of the record
        Key: string

        /// The location of the value in the ValueLog
        ValueLoc: int64
    }

    interface Collections.Generic.IComparer<SSTableRecord> with
        member this.Compare(x: SSTableRecord, y: SSTableRecord) : int = x.Key.CompareTo(y)

    /// +----------------------------------+
    /// | this.Key.Length (int) | this.Key |
    /// +----------------------------------+
    member public this.Size: int = 4 + this.Key.Length

/// On-disk String-Sorted Table(SSTable) of the keys.
/// On-disk storage of the keys and their value locations. Records in a SSTable are sorted
/// by their key as to support binary search. If a record isn't found in the MemTable, then
/// the database searches the SSTables starting with the lowest one in the hierarchy.
type SSTable(initPath: string) =
    /// Path of the SSTable on-disk.
    let __path: string = initPath

    /// Creation timestamp in microseconds and Compress level
    let (__timestamp, __level): uint64 * uint64 =
        SSTable.ParseTimestampAndLevel(initPath)

    /// File that the keys reside on.
    let __file: IO.FileStream =
        new IO.FileStream(initPath, IO.FileMode.Open, IO.FileAccess.Read)

    /// In-memory index of the location of the keys.
    let __records: Collections.Generic.List<int64> =
        new Collections.Generic.List<int64>()

    /// Size of the in-memory index.
    let __size: int = __records.Count

    /// Lowest key in the SSTable. Used to check if a key could possibly be in this SSTable.
    let mutable __low_key: string = ""

    /// Highest key in the SSTable. Used to check if a key could possibly be in this SSTable.
    let mutable __high_key: string = ""

    do
        let reader = new IO.BinaryReader(__file)
        let offset = ref 0L

        while true do
            if -1 = reader.PeekChar() then
                ()
            else
                let keyLength = reader.ReadBytes(8) |> BitConverter.ToUInt64
                let next = (keyLength |> int64) + 8L
                __file.Seek(next |> int64, IO.SeekOrigin.Current) |> ignore
                __records.Add(offset.Value) |> ignore
                offset.Value <- (offset.Value + next + 8L)

        __file.Seek(__records[0], IO.SeekOrigin.Begin) |> ignore
        let lowKeyLength = reader.ReadBytes(8) |> BitConverter.ToUInt64
        let valueLoc = reader.ReadBytes(8) |> BitConverter.ToInt64
        __low_key <- reader.ReadBytes(lowKeyLength |> int) |> Text.Encoding.UTF8.GetString

        let hightKeyLength = reader.ReadBytes(8) |> BitConverter.ToUInt64
        let valueLoc = reader.ReadBytes(8) |> BitConverter.ToInt64
        __file.Seek(__records[__records.Count - 1], IO.SeekOrigin.Begin) |> ignore
        __high_key <- reader.ReadBytes(hightKeyLength |> int) |> Text.Encoding.UTF8.GetString

    /// Creates a new SSTable from a full Memtbl.
    /// This consturctor will create a new SSTable at a path.
    /// If a file already exists at this path, then the file will be overwritten.
    /// The MemTable will not be freed after creating a new SSTable. The caller is responsible for freeing the MemTable.
    new(initPath: string, memtbl: Memtbl) =
        let writer =
            new IO.BinaryWriter(new IO.FileStream(initPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite))

        for record in memtbl do
            writer.Write(record.Key.Length |> uint64 |> BitConverter.GetBytes)
            writer.Write(record.ValueLoc |> BitConverter.GetBytes)
            writer.Write(record.Key |> Text.Encoding.UTF8.GetBytes)

        writer.Close()
        new SSTable(initPath)

    interface IDisposable with
        member this.Dispose() = __file.Close()

    /// Parses the creation timestamp and compaction level in microseconds from a SSTable filename.
    /// This function expects the filename in the format of `%ll-%ll.sstable`
    static public ParseTimestampAndLevel(filename: string) : uint64 * uint64 = 
        let regex = Text.RegularExpressions.Regex(@"(\d+)-(\d+)\.sstable")
        let matched = regex.Match(filename)
        (matched.Groups.[1].Value |> uint64, matched.Groups.[2].Value |> uint64)

    member private this.ReadRecord(offset: int64) =
        __file.Seek(offset, IO.SeekOrigin.Begin) |> ignore

        let reader = new IO.BinaryReader(__file)
        let keyLength = reader.ReadBytes(8) |> BitConverter.ToUInt64
        let valueLoc = reader.ReadBytes(8) |> BitConverter.ToInt64
        let key = reader.ReadBytes(keyLength |> int) |> Text.Encoding.UTF8.GetString

        { Key = key; ValueLoc = valueLoc }

    /// Gets the location of a value on the ValueLog from a key.
    /// This function uses the in-memory index to seek each record on disk.
    /// This function uses binary search for a runtime of `O(log(n))` seeks.
    member public this.GetValueLoc(key: string) =
        let a = ref 0
        let b = ref (__size - 1)

        let rec binarySearchKey () =
            if a < b then
                let m = a.Value + (b.Value - a.Value) / 2
                let record = this.ReadRecord(m)

                match record.Key.CompareTo(key) with
                | 0 -> Some(record.ValueLoc)
                | -1 ->
                    b.Value <- m - 1
                    binarySearchKey ()
                | 1 ->
                    a.Value <- m + 1
                    binarySearchKey ()
                | _ -> failwith "?"
            else
                None

        match binarySearchKey () with
        | Some valueLoc -> valueLoc
        | None ->
            let record = this.ReadRecord(a.Value)

            if record.Key.CompareTo(key) = 0 then
                record.ValueLoc
            else
                failwith "SSTable: Key {key} not found!"

    /// Checks if the given key could be in this SSTable.
    /// This function runs in constant time without any operations on disk.
    member public this.InKeyRange(key: string) =
        let lowLen =
            if __low_key.Length < key.Length then
                __low_key.Length
            else
                key.Length

        let cmp = key.CompareTo(__low_key)
        let lowKeyCmp = ref 0
        let highKeyCmp = ref 0

        if cmp <> 0 || __low_key.Length = key.Length then
            lowKeyCmp.Value <- cmp
        else
            lowKeyCmp.Value <- if key.Length < __low_key.Length then -1 else 1


        let highLen =
            if __high_key.Length < key.Length then
                __high_key.Length
            else
                key.Length

        let cmp = key.CompareTo(__high_key)

        if cmp <> 0 || __low_key.Length = key.Length then
            highKeyCmp.Value <- cmp
        else
            highKeyCmp.Value <- if key.Length < __high_key.Length then -1 else 1

        if lowKeyCmp.Value >= 0 && highKeyCmp.Value <= 0 then
            true
        else
            false
