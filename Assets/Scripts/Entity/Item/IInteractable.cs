using UnityEngine.EventSystems;

namespace Entity.Item {
  public interface IInteractable {
    public abstract void OnLeftClick();
    public abstract void OnMiddleClick();
    public abstract void OnRightClick();
  }
}