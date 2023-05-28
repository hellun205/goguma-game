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
    
    public Color effectColor = Color.white;

    public abstract ItemType type { get; }

    public virtual string GetTooltipText() =>
      $"{"name".GetStyleTag(nameColor.GetTag(_name))}\n" +
      $"{"type".GetStyleTag($"{type.GetString()} 아이템")}\n" +
      $"{GetStringTag("descriptions", descriptions)}";

    protected string Bar => "bar".GetStyleTag("───────────────────");

    protected string ValueColorTag(float value) => (value > 0 ? "increase" :"decrease").GetStyleTag(value.ToThousandsFormat());

    

    protected string GetStringTag(string style, string value) {
      return string.IsNullOrEmpty(value) ? String.Empty : style.GetStyleTag(value);
    }
    
  }
}