using Entity.Player;
using UnityEngine;

namespace Entity.Item.Useable {
  [CreateAssetMenu(fileName = "Potion", menuName = "Items/Useable/Potion")]
  public class Potion : UseableItem {
    public override UseableType u_type => UseableType.Potion;

    [Header("Potion")]
    public PlayerStatus Increase;
    
    public override void Use() {
      base.Use();
      var player = PlayerController.Instance;
      player.status += Increase;
    }
  }
}