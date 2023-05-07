/// When a record is written to our database, it is stored in two palaces: the MemTable and the WAL.
/// The WAL acts as an on-disk backup for the MemTable by keeping a running record of all of the database operations.
/// In the event of a restart, the MemTable can be fully recovered by replaying the operations in the WAL.
/// When a MemTable reaches capacity and is transformed into a SSTable, the WAL is wiped from the disk to make room for a new WAL.
module FSMDB.WAL

open System.IO
open System.IO.MemoryMappedFiles

/// RocksDB was using a block model, it would leave many padding bytes, wasting disk space.
/// But tools like MapReduce can take advantage of this block design and use it to split files into parts for batch processing jobs.
/// FSMDB's WAL design does not include block models or checksums like RocksDB.
/// Each of the entries will be stored back-to-back with only the necessary metadata to recover the keys and values of the records.
/// +---------------+---------------+-----------------+-...-+--...--+-----------------+
/// | Key Size (8B) | Tombstone(1B) | Value Size (8B) | Key | Value | Timestamp (8B)  |
/// +---------------+---------------+-----------------+-...-+--...--+-----------------+
/// - Key Size = Length of the Key data
/// - Tombstone = If this record was deleted and has a value
/// - Value Size = Length of the Value data
/// - Key = Key data
/// - Value = Value data
/// - Timestamp = Timestamp of the operation in microseconds
type WALEntry(key: WALKey, value: option<WALValue>, timestamp: int64, deleted: bool) =
    member public _.Key = key
    member public _.Value = value
    member public _.Timestamp = timestamp
    member public _.Deleted = deleted

and WALKey = string
and WALValue = string

/// Starting at the beginning of the WAL file, each entry is traversed, used to rebuild the MemTable on database restart.
type WALFileView(path: string) =
    // Check if the path exists
    do
        if not (Path.Exists(path)) then
            failwith $"""WALFileView: "{path}" does not exist!"""

    member public _.Path = path

    member public _.Value = WALFileView.Open(path)

    /// Using the memory-mapped IO of System.IO.MemoryMappedFiles can map files to memory
    /// Making file access as fast and efficient as accessing memory.
    /// The construction statement of WALIterator has checked the path.
    static member private Open(path: string) =
        // Named mapping is not supported in many non-Windows environments,
        // so the null parameter is used here to avoid the System.PlatformNotSupportedException exception
        use memoryMappedFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open, null)
        let fileView = memoryMappedFile.CreateViewAccessor()

        match Utils.Numeric.CheckInt64ToInt32(fileView.Capacity) with
        | Ok(capacity) ->
            let buffer = Array.zeroCreate<byte> (capacity)
            fileView.ReadArray(0, buffer, 0, capacity) |> ignore
            buffer
        | Error() -> failwith $"""WALFileView: File {path} too large"""

type WALFileIterator(view: WALFileView) =

    /// WALFileView.Value is an array<byte>, __pos points to the position of the current Iterator in it.
    let mutable __pos = 0
    member private _.View = view

    member private this.Advance() =
        if __pos >= this.View.Value.Length then
            None
        else
            __pos <- __pos + 1
            Some(view.Value[__pos])

    member private this.ReadTombstone() =
        match this.Advance() with
        | Some value -> System.Convert.ToBoolean(value)
        | None -> failwith "WALFileIterator: End Of File"

    member private this.ReadBytes(n: int) =
        let bytes = Array.zeroCreate<byte> (n)

        for i = 0 to 8 do
            match this.Advance() with
            | Some value -> bytes[i] <- value
            | None -> failwith "WALFileIterator: End Of File"

        bytes

    member private this.ReadWALKeyOrValueLen() =
        let key = this.ReadBytes(8)

        System.BitConverter.ToInt32(key)

    member public this.ReadEntry() =
        let key_len = this.ReadWALKeyOrValueLen()
        let value_len = this.ReadWALKeyOrValueLen()
        let deleted = this.ReadTombstone()

        WALEntry(
            System.BitConverter.ToString(this.ReadBytes(key_len)),
            (if value_len = 0 then
                 None
             else
                 Some(System.BitConverter.ToString(this.ReadBytes(value_len)))),
            System.BitConverter.ToInt64(this.ReadBytes(8)),
            deleted
        )

/// Write Ahead Log(WAL)
///
/// An append-only file that holds the operations performed on the MemTable.
/// The WAL is intended for recovery of the MemTable when the server is shutdown.
type WAL private (path: string) =
    member public _.Path = path

    static member Create(path: string) =
        Path.Combine([| path; Utils.Time.GetCurrentTimestamp().ToString(); ".wal" |])
