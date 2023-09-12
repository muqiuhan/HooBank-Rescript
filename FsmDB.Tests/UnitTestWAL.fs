module FsmDB.Tests.WAL

open NUnit.Framework
open FsmDB.WAL
open FsmDB.Memtbl
open System

[<TestFixture>]
type TestWAL() =
    [<Test>]
    static member ``Test initialize WAL``() =
        use wal = new WAL("wal.data")
        IO.File.Delete("wal.data")

    [<Test>]
    static member ``Test append WAL``() =
        use wal = new WAL("wal.data")
        let key = "apple"
        let value = "Apple Pie"

        wal.Add({ Key = key; ValueLoc = 0 })
        wal.Sync()

        use file =
            new IO.BinaryReader(new IO.FileStream("wal.data", IO.FileMode.Open, IO.FileAccess.Read))

        let keyLength = file.ReadBytes(8) |> BitConverter.ToUInt64
        let valueLoc = file.ReadBytes(8) |> BitConverter.ToInt64

        Assert.AreEqual(keyLength, key.Length)
        Assert.AreEqual(valueLoc, valueLoc)
        Assert.AreEqual(key, file.ReadBytes(keyLength |> int) |> Text.Encoding.UTF8.GetString)

        IO.File.Delete("wal.data")
