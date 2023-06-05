using System.Linq;
using Entity.Item;
using Entity.Player;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Inventory.QuickSlot
{
  public class QuickSlot : MonoBehaviour, IDropHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler,
                           IEndDragHandler
  {
    private Sprite noneSprite => Managers.Item.noneSprite;
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
    public QuickSlotController controller;

    [HideInInspector]
    public byte index;

    private Image drgImg => InventoryManager.Instance.dragImg;

    private void Awake()
    {
      inven.onItemChanged += InventoryItemChanged;
      rectTransform = GetComponent<RectTransform>();
    }

    private void InventoryItemChanged()
    {
      if (invenIndex.HasValue)
      {
        var item = inven[invenIndex.Value];

        if (item is null)
        {
          iconImg.sprite = noneSprite;
          countTMP.text = "";
        }
        else
        {
          iconImg.sprite = item.Value.item.sprite8x;
          iconImg.color = item.Value.item.spriteColor;
          countTMP.text = item.Value.count == 1 ? "" : item.Value.count.ToString();
        }
      }
      else
      {
        iconImg.sprite = noneSprite;
        countTMP.text = "";
      }

      controller.CallSlotChanged(); ;
    }

    public void SetIndex(byte? index = null)
    {
      invenIndex = index;
      InventoryItemChanged();
    }

    public void SetEnabled(bool enable)
    {
      var color = slotImg.color;
      color.a = enable ? 1f : 0.5f;
      slotImg.color = color;
      // iconImg.color = color;
    }

    public void OnDrop(PointerEventData eventData)
    {
      var invenCtrl = InventoryManager.Instance;

      if (invenCtrl.isDragging)
      {
        var draggedIdx = invenCtrl.dragedIdx;

        foreach (var slot in controller.slots.Where(slot => slot.invenIndex == draggedIdx))
          controller.AssignSlot(slot.index, null);
        controller.AssignSlot(index, draggedIdx);
      }
      else if (controller.isDragging)
      {
        var invIdx = controller.slots[controller.dragedIdx].invenIndex;

        if (invIdx is null)
          return;

        controller.AssignSlot(controller.dragedIdx, null);
        controller.AssignSlot(index, invIdx.Value);
      }

      controller.CallSlotChanged();
      Managers.Audio.PlaySFX("drag_item");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button == PointerEventData.InputButton.Left)
      {
        controller.SetIndex(index);
        Managers.Audio.PlaySFX("click");
      }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      if (invenIndex is null)
        return;
      drgImg.sprite = iconImg.sprite;
      drgImg.color = iconImg.color;
      controller.dragedIdx = index;
      controller.isDragging = true;

      drgImg.gameObject.SetActive(true);
      Managers.Audio.PlaySFX("drag_item");
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (invenIndex is null)
        return;

      drgImg.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      drgImg.gameObject.SetActive(false);
      controller.isDragging = false;
    }
  }
}
