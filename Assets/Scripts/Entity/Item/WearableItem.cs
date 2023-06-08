using System;
using UnityEngine;

namespace Entity.Item
{
  [CreateAssetMenu(fileName = "Wearable", menuName = "Item/Wearable")]
  public class WearableItem : BaseItem
  {
    public override ItemType type => ItemType.Wearable;

    public string test;
  }
}
