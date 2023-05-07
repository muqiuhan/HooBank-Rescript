module FSMDB.Tests.WAL

open NUnit.Framework
open FSMDB.WAL
open System

[<TestFixture>]
type TestWALFileView() =
    [<Test>]
    static member public FailedInitialize() =
        try
            WALFileView("") |> ignore
            Assert.Fail()
        with Failure _ ->
            Assert.Pass()

    [<Test>]
    static member public SuccessInitialize() =
        try
            match
                (WALFileView(IO.Path.Combine [| __SOURCE_DIRECTORY__; __SOURCE_FILE__ |]))
                    .Value.Length
            with
            | 0 -> Assert.Fail()
            | _ -> Assert.Pass()

        with Failure msg ->
            Assert.Warn(msg)
            Assert.Fail()
