using System;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils
{
  public static class StringUtils
  {
    public static string ToThousandsFormat(this float value) => $"{value:#,###}";

    public static string GetTag(this Color color, string text) => $"<color=#{color.ToHexString()}>{text}</color>";

    public static string GetStyleTag(this string style, string text) => $"<style=\"{style}\">{text}</style>";

    public static string GetValueTag(string text, float value)
    {
      if (value == 0) return string.Empty;

      var sb = new StringBuilder("<style=\"");
      var tag = (value > 0 ? "increase" : "decrease") + "\">";
      var symbol = (value > 0 ? "+" : "");

      sb.Append($"{tag}{text} {symbol}{Mathf.Round(value).ToThousandsFormat()}");
      sb.Append("</style>\n");
      return sb.ToString();
    }

    public static string GetTimeStr(this float time, string second = "s", string minute = "m", string hour = "h")
    {
      var intTime = Mathf.RoundToInt(time);
      int rem;
      int div;
      int value;
      string str;
      if (intTime >= 3600)
      {
        div = Math.DivRem(intTime, 3600, out rem);
        value = div;
        str = hour;
      }
      else if (intTime >= 60)
      {
        div = Math.DivRem(intTime, 60, out rem);
        value = div;
        str = minute;
      }
      else
      {
        value = intTime;
        str = second;
      }

      return $"{value}{str}";
    }
  }
}