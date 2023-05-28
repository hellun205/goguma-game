using Audio;
using Entity.Item.Useable;
using Entity.Player;
using Utils;
using Window;

namespace Entity.Item
{
  public abstract class UseableItem : Item, IInteractable
  {
    public delegate void _onUse(UseableItem item);

    public event _onUse onUse;

    public override ItemType type => ItemType.Useable;

    public abstract UseableType u_type { get; }

    public AudioData audioWhenUse;

    public float cooldown = 0f;

    public virtual void Use()
    {
      AudioManager.Play(audioWhenUse);
      onUse?.Invoke(this);
    }

    public virtual void OnRightClick()
    {
      WindowManager.Ask("아이템 사용",
        $"{nameColor.GetTag(_name)}(을)를 사용하시겠습니까?",
        "사용", "취소",
        use =>
        {
          if (use) Use();
        });
    }

    public void OnQuickClick()
    {
      Use();
    }

    protected void Consume(ushort count = 1)
    {
      PlayerController.Instance.inventory.LoseItem(this, count);
    }
  }
}
