using System;
using UnityEngine;

namespace Entity.Item
{
  [CreateAssetMenu(fileName = "Item", menuName = "Item/Normal")]
  public class NormalItem : Item
  {
    public override ItemType type => ItemType.Normal;
  }
}
