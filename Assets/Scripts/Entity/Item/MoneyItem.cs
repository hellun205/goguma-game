using UnityEngine;

namespace Entity.Item {
  [CreateAssetMenu(fileName = "Money", menuName = "Item/Money")]
  public class MoneyItem : Item {
    public override ItemType type => ItemType.Money;
  }
}