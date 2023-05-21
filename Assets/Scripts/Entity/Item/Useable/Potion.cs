using Entity.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Window;

namespace Entity.Item.Useable {
  [CreateAssetMenu(fileName = "Potion", menuName = "Item/Useable/Potion")]
  public class Potion : UseableItem {
    public override UseableType u_type => UseableType.Potion;

    [FormerlySerializedAs("Increase")]
    [Header("Potion")]
    public PlayerStatus increase;

    public override void Use() {
      base.Use();
      var player = PlayerController.Instance;
      player.status += increase;
      Consume();
    }

    public override string GetTooltipText() => base.GetTooltipText() + $"{Bar}\n" + increase.GetInfo();
    
  }
}