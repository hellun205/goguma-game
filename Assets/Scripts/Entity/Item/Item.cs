using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.Item {
  public abstract class Item : ScriptableObject {
    [Header("Item")]

    public string _name;
    
    public Color nameColor = Color.white;

    public string descriptions;

    [Header("Sprite")]
    public Sprite sprite;
    
    public Color spriteColor = Color.white;

    public abstract ItemType type { get; }
  }
}