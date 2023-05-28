using UnityEngine;

namespace Entity.Npc
{
  [CreateAssetMenu(fileName = "Shop", menuName = "Entity/Npc/Shop")]
  public class ShopNpc : Npc
  {
    public override NpcType type => NpcType.Shop;
  }
}
