/// Copyright (c) 2022 Muqiu Han
/// The MemTable (aka. Memory Table) is the in-memory cache of the latest set of record writes applied to the database.
module LSMDB.MemTable

open System
open System.Collections.Generic

type MemTableEntry(key: MemTableEntryKey, value: option<MemTableEntryValue>, timestamp: UInt128, deleted: bool) =
  member public _.Key = key
  member public _.Value = value

  /// The time this write occurred in microseconds and is used to order writes to the same key when cleaning our old data in SSTables.
  member public _.Timestamp = timestamp

  /// In order to support fast deletes, we have Tombstones for deleted records. With Tombstones.
  member public _.Deleted = deleted

and MemTableEntryKey = list<uint8>
and MemTableEntryValue = list<uint8>


type MemTable(entries: Dictionary<MemTableEntryKey, MemTableEntry>) =
  let mutable __size = 0
  member public _.Entries = entries
  member public _.Size = __size

  member public this.__Exists(key: MemTableEntryKey) =
    if this.Entries.ContainsKey(key) then Ok(()) else Error(())

  /// Set a Key-Value Pair in the MemTable
  /// If a Value existed on the deleted record, then add the difference of the new and old Value to the MemTable's size.
  member public this.Set(key: MemTableEntryKey, value: MemTableEntryValue, timestamp: UInt128) =
    let entry = MemTableEntry(key, Some(value), timestamp, false)

    match this.__Exists (key) with
    | Ok() ->
      Option.iter
        (fun (v: MemTableEntryValue) ->
          if v.Length > value.Length then
            __size <- __size - (v.Length - value.Length)
          else
            __size <- __size + (value.Length - v.Length))
        this.Entries[key].Value

      this.Entries[key] <- entry
    | Error(index) ->
      // Increase the size of the MemTable by the Key size, Value size, Timestamp size (16 bytes), Tombstone size (1 byte).
      __size <- __size + key.Length + value.Length + 17
      this.Entries[key] <- entry

  /// Delete a Key-Value Pair From the MemTable
  /// Even if the value doesn’t exist in the MemTable we have to insert the Tombstone record
  /// because this record may exist in the SSTables.
  /// Keeping Tombstones allows the database to clean up deleted records during Compaction.
  member public this.Delete(key: MemTableEntryKey, timestamp: UInt128) =
    let entry = MemTableEntry(key, None, timestamp, true)

    match this.__Exists (key) with
    | Ok() ->
      // If a Value existed on the deleted record, then subtract the size of the Value from the MemTable.
      Option.iter (fun (value: MemTableEntryValue) -> __size <- __size - value.Length) this.Entries[key].Value
      this.Entries[key] <- entry
    | Error() ->
      __size <- __size + key.Length + 17
      this.Entries[key] <- entry

  /// Fetch a MemTableEntry by Key
  member public this.Get(key: MemTableEntryKey) =
    Result.bind (fun () -> Ok(this.Entries[key])) (this.__Exists (key))
