using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils {
  public static class RandomUtils {
    public static T Random<T>(this IEnumerable<T> enumerable) {
      var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
      return enumerable1[UnityEngine.Random.Range(0, enumerable1.Count())];
    }
  }
}