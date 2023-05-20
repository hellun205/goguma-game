using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObject;
using UnityEngine;

namespace Entity.Item {
  public class ItemManager : ScriptableObjectManager<Item> {
    public Sprite noneSprite;

    public static ItemManager GetInstance() => (ItemManager)Instance;

  }
}