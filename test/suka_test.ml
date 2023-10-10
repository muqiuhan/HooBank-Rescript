open Alcotest

let _ =
  run "Suka Tests" [
    "Memtbl", Test_memtbl.suit
  ]