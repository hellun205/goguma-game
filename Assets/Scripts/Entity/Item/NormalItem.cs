using System;
using UnityEngine;

namespace Entity.Item {
  [CreateAssetMenu(fileName = "Item", menuName = "Items/Normal")]
  public class NormalItem : Item {
    public override ItemType type => ItemType.Normal;
  }
}