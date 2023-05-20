using System;
using Entity.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory {
  public class Slot : MonoBehaviour, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler {
    [HideInInspector]
    public Item item;

    [HideInInspector]
    public byte count = 1;

    [Header("UI Object")]
    [SerializeField]
    private Image iconImg;

    [SerializeField]
    private TextMeshProUGUI countTMP;

    private Button btn;

    private ItemToolTip toolTip => InventoryController.Instance.toolTipPanel;

    public void SetItem(Item item, byte count = 1) {
      this.item = item;
      this.count = count;

      iconImg.sprite = item.sprite8x;
      iconImg.color = item.spriteColor;
      countTMP.text = (count == 1 ? string.Empty : count.ToString());
    }

    private void Awake() {
      btn = GetComponent<Button>();
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
  }
}