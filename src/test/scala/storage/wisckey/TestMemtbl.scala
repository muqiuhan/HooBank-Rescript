package storage.wisckey

import storage.wisckey.memtbl.Memtbl
import storage.wisckey.memtbl.MemtblRecord

class TestMemtbl extends munit.FunSuite {
  test("Get and add records") {
    val memtbl = new Memtbl()
    val key1 = "OCaml"
    val value1 = "Real World OCaml"
    val value1Offset = 0L

    val key2 = "Scala"
    val value2 = "Functional Programming in Scala"
    val value2Offset = value1Offset + value1.length()

    memtbl.add(key1, value1Offset)
    assertEquals(memtbl.length, 1)
    assert(
      memtbl.get("OCaml") match {
        case Some(record) =>
          assertEquals(record.key, key1)
          assertEquals(record.valueLoc, value1Offset)
          true
        case None => false
      }
    )

    memtbl.add(MemtblRecord(key2, value2Offset))
    assertEquals(memtbl.length, 2)
    assert(
      memtbl.get("Scala") match {
        case Some(record) =>
          assertEquals(record.key, key2)
          assertEquals(record.valueLoc, value2Offset)
          true
        case None => false
      }
    )
  }

  test("Overrite records") {
    val memtbl = new Memtbl()
    val key1 = "OCaml"
    val value1 = "Real World OCaml"
    val value1Offset = 0L
    val overriteOffset = value1.length() + 1L

    memtbl.add(key1, value1Offset)
    memtbl.add(MemtblRecord(key1, overriteOffset))

    assertEquals(memtbl.length, 1)
    assert(
      memtbl.get("OCaml") match {
        case Some(record) =>
          assertEquals(record.key, key1)
          assertEquals(record.valueLoc, overriteOffset)
          true
        case None => false
      }
    )
  }

  test("Remove empty memtbl") {
    val memtbl = new Memtbl()

    assertEquals(memtbl.delete("OCaml"), ())
  }

  test("Remove record") {
    val memtbl = new Memtbl()
    val key1 = "OCaml"
    val value1 = "Real World OCaml"
    val value1Offset = 0L

    memtbl.add(key1, value1Offset)

    assertEquals(memtbl.length, 1)
    assertEquals(memtbl.delete("OCaml"), ())
    assertEquals(memtbl.get("OCaml"), None)
  }
}
