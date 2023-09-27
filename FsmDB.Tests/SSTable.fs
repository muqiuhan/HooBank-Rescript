module FsmDB.Tests.SSTable

open NUnit.Framework
open FsmDB.WAL
open FsmDB.Memtbl
open FsmDB.SSTable
open System

[<Test>]
let ``Initialize SSTable from Memtbl`` () =
    let path = "./123456789-1.sstable"
    let memtblSize = 1024
    let memtbl = new Memtbl()

    for i = 0 to memtblSize - 1 do
        memtbl.Add(
            { Key =
                [| (i >>> 24) &&& 0xFF; (i >>> 16) &&& 0xFF; (i >>> 8) &&& 0xFF; i &&& 0xFF |]
                |> Array.map byte
                |> Text.Encoding.ASCII.GetString
              ValueLoc = (i * 128) |> int64 }
        )

    Assert.AreEqual(memtblSize, memtbl.Count)

    use sstable = new SSTable(path, memtbl)
    Assert.AreEqual(memtblSize, sstable.Size)
    Assert.AreEqual("\x00\x00\x00\x00", sstable.LowKey)
    Assert.AreEqual("\x00\x00\x03\xff", sstable.HighKey)

    IO.File.Delete(path)