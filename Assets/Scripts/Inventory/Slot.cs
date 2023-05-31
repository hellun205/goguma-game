using System.Linq;
using Audio;
using Entity.Item;
using Entity.Player;
using Inventory.QuickSlot;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
  public class Slot : MonoBehaviour, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler,
                      IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
  {
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

    [SerializeField]
    private Sprite noneSprite;

    private QuickSlotController quickSlotCtrl => PlayerController.Instance.quickSlotCtrler;

    [SerializeField]
    private string animParameter;

    private Animator anim;

    [Header("sound")]
    [SerializeField]
    private AudioData dragSound;

    public void SetItem(Item item = null, byte count = 0)
    {
      this.item = item;
      this.count = count;

      var isNull = item is null;

      iconImg.sprite = isNull ? noneSprite : item.sprite8x;
      iconImg.color = isNull ? Color.clear : item.spriteColor;
      countTMP.text = isNull ? "" : (
        count == 1 ? string.Empty : count.ToString()
      );
    }

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (item == null)
        return;

      toolTip.gameObject.SetActive(true);
      anim.SetBool(animParameter, true);
      Refresh();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      toolTip.gameObject.SetActive(false);
      anim.SetBool(animParameter, false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
      toolTip.transform.position = eventData.position;
    }

    private void Refresh()
    {
      toolTip.itemData = item;
      toolTip.Refresh();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (item is not IInteractable interact)
        return;
      else if (eventData.button == PointerEventData.InputButton.Right)
        interact.OnRightClick();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      if (item is null)
        return;

      drgImg.sprite = item.sprite8x;
      drgImg.color = item.spriteColor;
      drgImg.gameObject.SetActive(true);
      inven.dragedIdx = index;
      inven.isDragging = true;
      // anim.SetBool(animParameter , false);
      Managers.Audio.PlaySFX("dragSound");
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (item is null)
        return;

      drgImg.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      drgImg.gameObject.SetActive(false);
      inven.isDragging = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
      if (inven.isDragging)
      {
        var list = quickSlotCtrl.slots.Where(x => x.invenIndex == inven.dragedIdx);

        foreach (var qSlot in list)
          qSlot.invenIndex = index;

        inven.inventory.Move(inven.dragedIdx, index);
      }
    }
  }
}
