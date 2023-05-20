using Entity.Player;
using UnityEngine;

namespace Entity.Item.Useable {
  [CreateAssetMenu(fileName = "Potion", menuName = "Item/Useable/Potion")]
  public class Potion : UseableItem {
    public override UseableType u_type => UseableType.Potion;

    [Header("Potion")]
    public PlayerStatus Increase;

    public override void Use() {
      base.Use();
      var player = PlayerController.Instance;
      player.status += Increase;
    }

    public override string GetTooltipText() =>
      base.GetTooltipText() +
      $"{Bar}\n" +
      GetValueTag("최대 체력", Increase.maxHp) +
      GetValueTag("체력", Increase.hp) +
      GetValueTag("이동 속도", Increase.moveSpeed) +
      GetValueTag("점프력", Increase.jumpSpeed);
  }
}