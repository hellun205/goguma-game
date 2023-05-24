using Entity.Item;
using Entity.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory {
  public class Slot : MonoBehaviour, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler,
                      IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
    [HideInInspector]
    public Item item;

    [HideInInspector]
    public byte count = 1;

    [Header("UI Object")]
    [SerializeField]
    private Image iconImg;

    [SerializeField]
    private TextMeshProUGUI countTMP;

    private ItemToolTip toolTip => InventoryController.Instance.toolTipPanel;
    
    private Image drgImg => InventoryController.Instance.dragImg;

    [HideInInspector]
    public byte index;

    private InventoryController inven => InventoryController.Instance;

    public void SetItem(Item item, byte count = 1) {
      this.item = item;
      this.count = count;

      iconImg.sprite = item.sprite8x;
      iconImg.color = item.spriteColor;
      countTMP.text = (count == 1 ? string.Empty : count.ToString());
    }

    private void Awake() {

    }

    public void OnPointerEnter(PointerEventData eventData) {
      if (item == null) return;
      toolTip.gameObject.SetActive(true);
      Refresh();
    }

    public void OnPointerExit(PointerEventData eventData) {
      toolTip.gameObject.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData) {
      toolTip.transform.position = eventData.position;
    }

    private void Refresh() {
      toolTip.itemData = item;
      toolTip.Refresh();
    }

    public void OnPointerClick(PointerEventData eventData) {
      if (item is not IInteractable interact) return;
      if (eventData.button == PointerEventData.InputButton.Right) {
        interact.OnRightClick();
      }
    }

    public void OnBeginDrag(PointerEventData eventData) {
      drgImg.sprite = item.sprite8x;
      drgImg.gameObject.SetActive(true);
      inven.dragedIdx = index;
      inven.isDraging = true;
    }

    public void OnDrag(PointerEventData eventData) {
      drgImg.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
      drgImg.gameObject.SetActive(false);
      inven.isDraging = false;
    }

    public void OnDrop(PointerEventData eventData) {
      Debug.Log("drop slot");
    }
  }
}