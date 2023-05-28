using System;
using UnityEngine;

namespace Entity.Item
{
  [CreateAssetMenu(fileName = "Wearable", menuName = "Item/Wearable")]
  public class WearableItem : Item
  {
    public override ItemType type => ItemType.Wearable;

    public string test;
  }
}
