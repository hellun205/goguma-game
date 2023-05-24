using Entity.Item;
using Entity.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Window;

namespace Inventory {
  public class ThrowCheck : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
      var invenCtrl = InventoryController.Instance;
      var inven = invenCtrl.inventory;
      if (!invenCtrl.isDraging) return;
      var item = inven[invenCtrl.dragedIdx];
      WindowManager.Ask("버리기", $"{item.item._name}(을)를 버리시겠습니까?", callback: @throw => {
        if (item.count == 1) {
          Throw(item.item, item.count);
        } else {
          var maxCnt = inven.ItemCount(item.item);
          WindowManager.ReadInt("버리기", cnt => Throw(item.item, (ushort) cnt), 
            $"몇개를 버리시겠습니까? (최대 {maxCnt} 개)", maxCnt, minValue: 0, maxValue: maxCnt);
        }
      });
    }

    private static void Throw(Item item, ushort count) {
      var player = PlayerController.Instance;
      player.inventory.LoseItem(item, count);
      player.ThrowItem(item, count);
    }
  }
}