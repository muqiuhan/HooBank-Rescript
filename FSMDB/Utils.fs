module FSMDB.Utils

open System

type Time =
    static member public GetCurrentTimestamp() = DateTime.Now.Ticks

type Numeric =

    /// Checks whether conversion to type Int32 is possible without overflow.
    static member public checkInt64ToInt32(n: int64) =
        if n > int64 (Int32.MaxValue) || n < int64 (Int32.MinValue) then
            Error()
        else
            Ok(int32 (n))
