using UnityEngine;

namespace Entity.Npc {
  [CreateAssetMenu(fileName = "Npc", menuName = "Entity/Npc/Normal")]
  public class NormalNpc : Npc {
    public override NpcType type => NpcType.None;
  }
}