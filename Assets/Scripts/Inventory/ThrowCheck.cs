using System.Linq;
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
      var quickCtrl = PlayerController.Instance.quickSlotCtrler;
      
      if (invenCtrl.isDragging) {
        var item = inven[invenCtrl.dragedIdx];
        if (item is null) return;
        WindowManager.Ask("버리기", $"{item.Value.item._name}(을)를 버리시겠습니까?", callback: @throw => {
          if (!@throw) return;
          if (item.Value.count == 1) {
            Throw(item.Value.item, item.Value.count);
          } else {
            var maxCnt = inven.ItemCount(item.Value.item);
            WindowManager.ReadInt("버리기", cnt => Throw(item.Value.item, (ushort) cnt),
              $"몇개를 버리시겠습니까? (최대 {maxCnt} 개)", maxCnt, minValue: 0, maxValue: maxCnt);
          }
        });
      } else if (quickCtrl.isDragging) {
        quickCtrl.AssignSlot(quickCtrl.dragedIdx, null);
      }
    }

    private static void Throw(Item item, ushort count) {
      var player = PlayerController.Instance;
      var check = player.quickSlotCtrler.slots
       .Where(x => x.invenIndex.HasValue && x.inven[x.invenIndex.Value].Value.item == item);
      if (check.Any()) {
        foreach (var qSlot in check) {
          qSlot.SetIndex();
        }
      }

      player.inventory.LoseItem(item, count);
      player.ThrowItem(item, count);
    }
  }
}