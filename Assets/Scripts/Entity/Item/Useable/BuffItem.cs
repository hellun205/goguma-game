using Buff;
using Entity.Player;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Window;

namespace Entity.Item.Useable
{
  [CreateAssetMenu(fileName = "BuffItem", menuName = "Item/Useable/Buff")]
  public class BuffItem : UseableItem
  {
    public override UseableType u_type => UseableType.Buff;

    [Header("Buff")]
    public PlayerStatus increase;

    public ushort time;

    public bool canDuplicateUse;

    public override void Use()
    {
      base.Use();
      if (!canDuplicateUse && BuffManager.Instance.HasBuff(this))
      {
        WindowManager.Show("아이템 사용", "이미 사용 중 입니다.");
        return;
      }
      BuffManager.Instance.Add(this);
      Consume();
    }

    public override string GetTooltipText() => base.GetTooltipText() + $"{Bar}\n" + increase.GetInfo();
  }
}
