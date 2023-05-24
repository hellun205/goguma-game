using Entity.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Inventory.QuickSlot {
  public class QuickSlot : MonoBehaviour, IDropHandler {
    public Inventory inven => PlayerController.Instance.inventory;

    public byte? invenIndex = null;

    [HideInInspector]
    public byte count = 1;

    [Header("UI Object")]
    [SerializeField]
    private Image slotImg;

    [SerializeField]
    private Image iconImg;

    [SerializeField]
    private TextMeshProUGUI countTMP;
    
    public RectTransform rectTransform { get; private set; }

    [HideInInspector]
    public byte index;

      private void Awake() {
      inven.onItemChanged += InventoryItemChanged;
      rectTransform = GetComponent<RectTransform>();
    }

    private void InventoryItemChanged() {
      if (!invenIndex.HasValue) return;
      var item = inven.items[invenIndex.Value];
      iconImg.sprite = item.item.sprite8x;
      countTMP.text = item.count.ToString();
    }

    public void SetIndex(byte? index = null) {
      invenIndex = index;
      InventoryItemChanged();
    }

    public void SetEnabled(bool enable) {
      var color = slotImg.color;
      color.a = enable ? 1f : 0.5f;
      slotImg.color = color;
      iconImg.color = color;
    }

    public void OnDrop(PointerEventData eventData) {
      var invenCtrl = InventoryController.Instance;
      if (!invenCtrl.isDraging) return;
      var quickCtrl = PlayerController.Instance.quickSlotCtrler;
      var draggedIdx = invenCtrl.dragedIdx;
      quickCtrl.AssignSlot(index, draggedIdx);
    }
  }
}