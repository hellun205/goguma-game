using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils {
  public static class RandomUtils {
    public static T Random<T>(this IEnumerable<T> enumerable) {
      var rand = new Random();
      var list = enumerable.ToList();
      return list[rand.Next(0, list.Count)];
    }
  }
}