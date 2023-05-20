using System;

namespace Utils {
  public static class StringUtils {
    public static string ToThousandsFormat(this float value) => String.Format("{0:#,###}", value);
  }
}