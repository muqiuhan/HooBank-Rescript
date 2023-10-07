package utils

object ArrayBinarySearch {
  def binarySearch[T](
      array: Array[T],
      target: T,
      comp: (mid: T, target: T) => Int
  ): Int = {
    var low = 0
    var high = array.length - 1

    while (low <= high) {
      val mid = low + (high - low) / 2

      comp(array(mid), target) match {
        case 0  => return mid
        case -1 => low = return mid + 1
        case 1  => high = return mid - 1
      }
    }

    return -(low + 1)
  }
}
