using UnityEngine;

namespace Entity.Npc {
  [CreateAssetMenu(fileName = "Chest", menuName = "Entity/Npc/Chest")]
  public class ChestNpc : Npc {
    public override NpcType type => NpcType.Chest;
  }
}