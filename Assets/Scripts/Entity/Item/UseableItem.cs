using Entity.Item.Useable;

namespace Entity.Item {
  public abstract class UseableItem : Item, IInteractable {
    public delegate void _onUse(UseableItem item);

    public event _onUse onUse;
    
    public override ItemType type => ItemType.Useable;
    
    public abstract UseableType u_type { get; }

    public virtual void Use() {
      onUse?.Invoke(this);
    }

    public virtual void OnLeftClick() {
    }
    public virtual void OnMiddleClick() {
    }
    public virtual void OnRightClick() {
    }
  }
}