module FsmDB.Tests.WAL

open NUnit.Framework
open FsmDB.WAL
open FsmDB.Memtbl
open System

[<Test>]
let ``Initialize WAL`` () =
    use wal = new WAL("wal.data")
    IO.File.Delete("wal.data")

[<Test>]
let ``Append to WAL`` () =
    use wal = new WAL("wal.data")
    let key = "apple"
    let value = "Apple Pie"

    wal.Add(MemtblRecord(key, 0))
    wal.Sync()

    use file =
        new IO.BinaryReader(new IO.FileStream("wal.data", IO.FileMode.Open, IO.FileAccess.Read))

    let keyLength = file.ReadBytes(8) |> BitConverter.ToUInt64
    let valueLoc = file.ReadBytes(8) |> BitConverter.ToInt64

    Assert.AreEqual(keyLength, key.Length)
    Assert.AreEqual(valueLoc, valueLoc)
    Assert.AreEqual(key, file.ReadBytes(keyLength |> int) |> Text.Encoding.UTF8.GetString)

    IO.File.Delete("wal.data")
