using UnityEngine;

namespace Entity.Item {
  [CreateAssetMenu(fileName = "Money", menuName = "Items/Money")]
  public class MoneyItem : Item {
    public override ItemType type => ItemType.Money;
  }
}