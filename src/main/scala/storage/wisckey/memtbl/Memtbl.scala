package storage.wisckey.memtbl

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
  // private val size = 0

  private val records = new collection.concurrent.TrieMap[String, Long]()

  /** Gets a MemTableRecord from a MemTable by key. */
  def get(key: String): Option[MemtblRecord] = {
    records.get(key).map(valueLoc => MemtblRecord(key, valueLoc))
  }

  /** Sets a key-value pair in a MemTable. */
  def add(key: String, valueLoc: Long) = {
    records.addOne(key, valueLoc)
  }

  /** Sets a key-value pair in a MemTable. */
  def add(record: MemtblRecord) = {
    records.addOne(record.key, record.valueLoc)
  }

  /** Deletes a record from a MemTable. */
  def delete(record: MemtblRecord) = {
    val _ = records.remove(record.key)
  }

  /** Deletes a record from a MemTable. */
  def delete(key: String) = {
    val _ = records.remove(key)
  }
}
case object Memtbl {

  /** Max number of MemTableRecords in a MemTable */
  def MEMTBL_SIZE: Int = 1024
}
