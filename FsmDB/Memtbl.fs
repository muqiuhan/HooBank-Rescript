(*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2022 Muqiu Han
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *)

/// In-memory table of the records that have been modified most recently.
module FsmDB.Memtbl

open SkipList


/// Single Record in the MemTable.
/// Each records holds the key and the position of the record in the Value log. *)
type Record =
    {
        /// The key of the record
        key: string

        /// The location of the value in the ValueLog
        value_loc: int64
    }

    static member private Compare() = ()

/// In-memory table of the database.
/// In-memory table of the records that have been modified most recently. At any given
/// time, there is only one active MemTable in the database engine. The MemTable is always
/// the first store to be searched when a key-value pair is requested. *)
type Memtbl() =

    /// SkipList of records sorted by key
    let __records: SkipList<Record> = new SkipList<Record>()
