using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils
{
  public static class String
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
      int div;
      int value;
      string str;
      switch (intTime)
      {
        case >= 3600:
          div = Math.DivRem(intTime, 3600, out _);
          value = div;
          str = hour;
          break;

        case >= 60:
          div = Math.DivRem(intTime, 60, out _);
          value = div;
          str = minute;
          break;

        default:
          value = intTime;
          str = second;
          break;
      }

      return $"{value}{str}";
    }

    public static readonly string EndTag = "$";

    public static readonly Dictionary<string, Color> Colors = new()
    {
      { "$4", new Color32(0xaa, 0x00, 0x00, 0xff) },
      { "$c", new Color32(0xff, 0x55, 0x55, 0xff) },
      { "$6", new Color32(0xff, 0xaa, 0x0, 0xff) },
      { "$e", new Color32(0xff, 0xff, 0x55, 0xff) },
      { "$2", new Color32(0x00, 0xaa, 0x00, 0xff) },
      { "$a", new Color32(0x55, 0xff, 0x55, 0xff) },
      { "$b", new Color32(0x55, 0xff, 0xff, 0xff) },
      { "$3", new Color32(0x00, 0xaa, 0xaa, 0xff) },
      { "$1", new Color32(0x00, 0x00, 0xaa, 0xff) },
      { "$9", new Color32(0x55, 0x55, 0xff, 0xff) },
      { "$d", new Color32(0xff, 0x55, 0xff, 0xff) },
      { "$5", new Color32(0xaa, 0x00, 0xaa, 0xff) },
      { "$f", new Color32(0xff, 0xff, 0xff, 0xff) },
      { "$7", new Color32(0xaa, 0xaa, 0xaa, 0xff) },
      { "$8", new Color32(0x55, 0x55, 0x55, 0xff) },
      { "$0", new Color32(0x00, 0x00, 0x00, 0xff) },
    };

    public static string ToColorTags(this string text)
    {
      foreach (var (tag, color) in Colors)
      {
        if (!text.Contains(tag)) continue;

        text = text.Replace(tag, $"<color=#{color.ToHexString()}>");
      }

      if (text.Contains(EndTag))
        text = text.Replace(EndTag, "</color>");

      return text;
    }
  }
}
