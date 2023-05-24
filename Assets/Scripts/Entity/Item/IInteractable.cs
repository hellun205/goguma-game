using UnityEngine.EventSystems;

namespace Entity.Item {
  public interface IInteractable {
    public void OnLeftClick();
    public void OnMiddleClick();
    public void OnRightClick();
  }
}