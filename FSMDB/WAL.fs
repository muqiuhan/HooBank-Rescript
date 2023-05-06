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
/// | Key Size (8B) | Tombstone(1B) | Value Size (8B) | Key | Value | Timestamp (16B) |
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
type WALIterator(path: string) =
    // Check if the path exists
    do
        if not (Path.Exists(path)) then
            failwith $"""WALIterator: "{path}" does not exist!"""

    member public _.Path = path

    member private _.File = WALIterator.Open(path)

    /// Using the memory-mapped IO of System.IO.MemoryMappedFiles can map files to memory
    /// Making file access as fast and efficient as accessing memory.
    /// The construction statement of WALIterator has checked the path.
    static member private Open(path: string) =
        use memoryMappedFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open, path)
        let fileView = memoryMappedFile.CreateViewAccessor()

        match Utils.Numeric.CheckInt64ToInt32 (fileView.Capacity) with
        | Ok(capacity) ->
            let buffer = Array.zeroCreate<byte> (capacity)
            fileView.ReadArray(0, buffer, 0, capacity) |> ignore
            buffer
        | Error() -> failwith $"""WALIterator: File {path} too large"""

    