open Alcotest

let key1 = "Luoxi"
let value1 = 0L
let key2 = "Muqiu"
let value2 = 6L
let key3 = "Ababa"
let value3 = 12L

let put_and_get () =
  let memtbl = Memtbl.create () in
    Memtbl.put memtbl ~key:key1 ~value_loc:value1;

    (check string) "same key" key1 (Memtbl.get_exn memtbl ~key:key1).key;
    (check int64) "same value" value1
      (Memtbl.get_exn memtbl ~key:key1).value_loc;

    (check int) "correct length" 1 (Memtbl.length memtbl);

    Memtbl.put memtbl ~key:key2 ~value_loc:value2;
    (check string) "same key" key1 (Memtbl.get_exn memtbl ~key:key2).key;
    (check int64) "same value" value2
      (Memtbl.get_exn memtbl ~key:key2).value_loc;

    (check int) "correct length" 2 (Memtbl.length memtbl)

let suit = [
  test_case "Put and get value" `Quick put_and_get
]