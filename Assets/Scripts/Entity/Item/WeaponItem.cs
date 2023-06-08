using Entity.Player.Attack;
using UnityEngine;

namespace Entity.Item
{
  [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
  public class WeaponItem : BaseItem
  {
    public override ItemType type => ItemType.Weapon;

    [Header("Weapon")]
    public Hands weaponType;

    public Sprite weaponSprite;

    public Skill skill;
  }
}
