package storage.wisckey.memtbl

import java.util.concurrent.ConcurrentSkipListMap
import java.util.concurrent.ConcurrentHashMap

/** In-memory table of the records that have been modified most recently. At any
  * given time, there is only one active Memtbl in the database engine. The
  * MemTable is always the first store to be searched when a key-value pair is
  * requested.
  */
class Memtbl() {

  /** The size method of ConcurrentSkipListSet is not a constant time complexity
    * operation. It needs to traverse all elements to determine the size of the
    * set, so it may be very slow if it contains a large number of elements.
    */
  private var size = 0

  val records = new ConcurrentHashMap[String, Long]()

  /** Gets a MemTableRecord from a MemTable by key. */
  def get(key: String): Option[MemtblRecord] = {
    val valueLoc: Long | Null = records.get(key)

    valueLoc match {
      case valueLoc: Long => Some(MemtblRecord(key, valueLoc))
      case _              => None
    }
  }

  /** Sets a key-value pair in a MemTable. */
  def add(key: String, valueLoc: Long) = {
    size += 1
    records.put(key, valueLoc)
  }

  /** Sets a key-value pair in a MemTable. */
  def add(record: MemtblRecord) = {
    size += 1
    records.put(record.key, record.valueLoc)
  }

  /** Deletes a record from a MemTable. */
  def delete(record: MemtblRecord) = {
    size -= 1
    val _ = records.remove(record.key)
  }

  /** Deletes a record from a MemTable. */
  def delete(key: String) = {
    size -= 1
    val _ = records.remove(key)
  }

  def length: Int = records.size
}
case object Memtbl {

  /** Max number of MemTableRecords in a MemTable */
  def MEMTBL_SIZE: Int = 1024
}
