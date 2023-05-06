module FSMDB.Utils

open System

type Time =
    static member public GetCurrentTimestamp() = DateTime.Now.Ticks
