open Core
open Stdint

(** Single Record in the MemTable. *)
module Record = struct
  type t = {
    key : key;
    value_loc : value;
  }
  [@@deriving show, sexp]

  and key = string [@@deriving show, sexp]

  and value = int64 [@@deriving show, sexp]
  (** Each MemTableRecord holds the key and the position of the record in the ValueLog.
      [key] is the key of the record.
      [value_loc] is the location of the value in the ValueLog. *)
end

include Hashtbl

type t = (Record.key, Record.value) Hashtbl.t
(** At any given time, there is only one active MemTable in the database engine.
    The MemTable is always the first store to be searched when a key-value pair is requested. *)

let create () : t = Hashtbl.create (module String)

let[@inline always] put memtbl ~(key : Record.key) ~(value_loc : Record.value) =
  Hashtbl.add memtbl ~key ~data:value_loc |> ignore

let[@inline always] put_record memtbl (record : Record.t) =
  Hashtbl.add memtbl ~key:record.key ~data:record.value_loc |> ignore

let[@inline always] get memtbl ~(key : Record.key) =
  Option.(Hashtbl.find memtbl key >>| fun value_loc -> Record.{key; value_loc})

let[@inline always] get_exn memtbl ~(key : Record.key) =
  Hashtbl.find_exn memtbl key |> fun value_loc -> Record.{key; value_loc}

let[@inline always] get_value memtbl ~(key : Record.key) =
  Option.(Hashtbl.find memtbl key >>| fun value_loc -> value_loc)

let[@inline always] get_value_exn memtbl ~(key : Record.key) =
  Hashtbl.find_exn memtbl key
