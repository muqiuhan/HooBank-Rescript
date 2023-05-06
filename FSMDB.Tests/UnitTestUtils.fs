module FSMDB.Tests.Utils

open FSMDB.Utils
open NUnit.Framework
open System

[<TestFixture>]
type TestNumberic() =
    [<Test>]
    static member public CheckInt64ToInt32() =
        match Numeric.CheckInt64ToInt32(int64 (Int32.MaxValue) + 1L) with
        | Ok(_) -> Assert.Fail()
        | Error() -> Assert.Pass()

        match Numeric.CheckInt64ToInt32(int64 (Int32.MinValue) - 1L) with
        | Ok(_) -> Assert.Fail()
        | Error() -> Assert.Pass()
