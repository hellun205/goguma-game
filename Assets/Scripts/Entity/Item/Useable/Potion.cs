using Entity.Player;
using UnityEngine;
using Utils;
using Window;

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

    public override string GetTooltipText() => base.GetTooltipText() + $"{Bar}\n" + GetIncreaseText();

    public string GetIncreaseText() =>
      GetValueTag("최대 체력", Increase.maxHp) +
      GetValueTag("체력", Increase.hp) +
      GetValueTag("이동 속도", Increase.moveSpeed) +
      GetValueTag("점프력", Increase.jumpSpeed);

    public override void OnRightClick() {
      base.OnRightClick();
      WindowManager.Ask("아이템 사용",
        $"{nameColor.GetTag(_name)}(을)를 사용하시겠습니까?\n사용 시:\n{GetIncreaseText()}",
        "사용", "취소",
        use => {
          if (!use) return;
          Use();
          WindowManager.Show("아이템 사용", $"{nameColor.GetTag(_name)}(을)를 사용하였습니다.");
        });
    }
  }
}