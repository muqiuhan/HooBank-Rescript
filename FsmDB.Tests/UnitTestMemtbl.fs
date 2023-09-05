module FsmDB.Tests.Memtbl

open FsmDB.Memtbl
open NUnit.Framework

[<TestFixture>]
type TestMemtbl() =

    [<Test>]
    static member ``Test Memtbl set start``() =
        let memtbl = new Memtbl()
        let record1 = { Key = "lime"; ValueLoc = 0 }
        let record2 = { Key = "cherry"; ValueLoc = 0 }
        let record3 = { Key = "lime"; ValueLoc = 0 }
        memtbl.Add(record1)

        Assert.AreEqual(1, memtbl.Count)
        Assert.AreEqual(record1, memtbl.GetFirst())

        memtbl.Add(record2)
        Assert.AreEqual(2, memtbl.Count)
        Assert.AreEqual(record2, memtbl.GetFirst())
        Assert.AreEqual(record1, memtbl.GetLast())