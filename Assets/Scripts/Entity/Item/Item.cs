using System;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Entity.Item {
  public abstract class Item : UnityEngine.ScriptableObject {
    [Header("Item")]
    public string _name;

    public Color nameColor = Color.white;

    [Multiline]
    public string descriptions;

    [Header("Sprite")]
    public Sprite sprite;

    public Sprite sprite8x;

    public Color spriteColor = Color.white;

    public abstract ItemType type { get; }

    public virtual string GetTooltipText() =>
      $"<style=\"name\">{GetColorTag(nameColor)}{_name}</style>\n" +
      $"<style=\"type\">{type.GetString()} 아이템</style>\n" +
      GetStringTag("descriptions", descriptions);


    protected string GetColorTag(Color color) => $"<color=#{color.ToHexString()}>";

    protected string Bar => $"<style=\"bar\">───────────────────</style>";

    protected string ValueColorTag(float value) => (value > 0 ? "<style=\"increase\">" : "<style=\"decrease\">");

    protected string GetValueTag(string text, float value) {
      if (value == 0) return string.Empty;

      var sb = new StringBuilder("<style=\"");
      var tag = (value > 0 ? "increase" : "decrease") + "\">";
      var symbol = (value > 0 ? "+" : "");

      sb.Append($"{tag}{text} {symbol}{Mathf.Round(value).ToThousandsFormat()}");
      sb.Append("</style>\n");
      return sb.ToString();
    }

    protected string GetStringTag(string style, string value) {
      if (string.IsNullOrEmpty(value)) return String.Empty;
      return $"<style=\"{style}\">{value}</style>\n";
    }
    
  }
}