using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils {
  public static class StringUtils {
    public static string ToThousandsFormat(this float value) => String.Format("{0:#,###}", value);

    public static string GetTag(this Color color, string text) => $"<color=#{color.ToHexString()}>{text}</color>";
  }
}