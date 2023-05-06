module FSMDB.Tests.WAL

open NUnit.Framework
open FSMDB.WAL
open System

[<TestFixture>]
type TestWALIterator() =
    [<Test>]
    static member public FailedInitialize() =
        try
            WALIterator("") |> ignore
            Assert.Fail()
        with Failure _ ->
            Assert.Pass()

    [<Test>]
    static member public SuccessInitialize() =
        try
            WALIterator(IO.Path.Combine [| __SOURCE_DIRECTORY__; __SOURCE_FILE__ |])
            |> ignore

            Assert.Pass()
        with Failure msg ->
            Assert.Warn(msg)
            Assert.Fail()
