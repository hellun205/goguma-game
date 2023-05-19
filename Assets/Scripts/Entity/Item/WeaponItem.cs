using UnityEngine;

namespace Entity.Item {
  [CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon")]
  public class WeaponItem : Item {
    public override ItemType type => ItemType.Weapon;
  }
}