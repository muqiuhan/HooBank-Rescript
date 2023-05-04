open Stdint
open Core

(** MemTableEntrys with the modified record information. *)
module Entry = struct
  (** @timestamp is the time write occurred in microseconds and is used
   ** to order writes to the same key when cleaning old data in SSTbl.
   ** In order to support fast deletes, we have @deleted for deleted records. *)
  type t =
    { key : uint8 list
    ; value : uint8 list option
    ; timestamp : uint128
    ; deleted : bool
    }
end

(** Skip list (or skiplist) is a probabilistic data structure that allows
 ** O(log(n)) average complexity for search as well as O(log(n)) average
 ** complexity for insertion within an ordered sequence of n n elements *)
module List = struct
  include List

  let insert (lst : 'a t) (index : int) (entry : Entry.t) =
    let index = ref index in
    List.map lst ~f:(fun e ->
      if !index = 0
      then entry
      else (
        decr index;
        e))
  ;;
end

(** The Memtbl (aka. Memory Table) is the in-memory cache of 
 ** the latest set of record writes applied to the database 
 ** before it’s flushed into SSTable or SST files *)
type t =
  { entries : Entry.t List.t
  ; size : uint32
  }

(** Create new Memtbl *)
let create () = { entries = []; size = Uint32.of_int 0 }

(** Find a record in the Memtbl. *)
let get_index (db : t) (key : uint8 list) =
  List.fold db.entries ~init:(-1) ~f:(fun index entry ->
    if List.equal (fun x y -> Uint8.compare x y = 0) entry.key key
    then index + 1
    else index)
  |> function
  | -1 -> None
  | index -> Some index
;;

(** Sets a K-V pair in the Memtbl
 ** if a value existed on the deleted record, 
 ** then add the difference of the new and old value to the @db.size *)
let set (db : t) (k : uint8 list) (v : uint8 list) (timestamp : uint128) =
  let entry = Entry.{ key = k; value = Some v; timestamp; deleted = false } in
  match get_index db k with
  | Some index ->
    Option.(
      List.nth db.entries index
      >>= fun entry ->
      entry.value
      >>| (fun entry_value ->
            let entry_value_len = List.length entry_value
            and value_len = List.length v in
            if entry_value_len > value_len
            then { db with size = Uint32.of_int (entry_value_len - value_len) }
            else { db with size = Uint32.of_int (entry_value_len + value_len) })
      >>| fun db -> { db with entries = List.insert db.entries index entry })
  | None ->
    Some { entries = entry :: db.entries; size = Uint32.of_int (List.length k + 16 + 1) }
;;