package storage.wisckey.memtbl

import java.util.Comparator

/** Single Record in the MemTable. Each MemTableRecord holds the key and the
  * position of the record in the ValueLog.
  */
class MemtblRecord(
    /** The key of the record. */
    val key: String,

    /** The location of the value in the ValueLog. */
    val valueLoc: Long
) {
  def sizeof: Int = key.length() + 16
}

object MemtblRecord {
  def comparator = new Comparator[MemtblRecord] {
    override def compare(x: MemtblRecord, y: MemtblRecord): Int = {
      x.key.compareTo(y.key)
    }
  }
}
