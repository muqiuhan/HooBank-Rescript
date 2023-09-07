module FsmDB.Tests.ValueLog

open FsmDB.ValueLog
open NUnit.Framework
open System

[<TestFixture>]
type TestValueLog() =
    [<Test>]
    static member ``Test initialize ValueLog``() =
        let log = new ValueLog("value_log.data", 0u, 128u)
        Assert.AreEqual(0u, log.Head)
        Assert.AreEqual(128u, log.Tail)

        IO.File.Delete("value_log.data")

    [<Test>]
    static member ``Test append to ValueLog``() =
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
