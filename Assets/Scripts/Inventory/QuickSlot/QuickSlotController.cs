using System.Collections.Generic;
using Entity.Item;
using Entity.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory.QuickSlot
{
  public class QuickSlotController : MonoBehaviour
  {
    public List<QuickSlot> slots = new List<QuickSlot>();

    public Inventory inven => PlayerController.Instance.inventory;

    public byte slotIndex { get; private set; }

    public delegate void _onSlotChanged(byte slotIndex);

    public event _onSlotChanged onSlotChanged;

    [HideInInspector]
    public bool isDragging;

    [HideInInspector]
    public byte dragedIdx;

    [HideInInspector]
    public byte previousIndex;

    private void Awake()
    {
      slots.AddRange(GetComponentsInChildren<QuickSlot>());

      for (var i = 0; i < slots.Count; i++)
      {
        slots[i].index = (byte)i;
        slots[i].controller = this;
      }
    }

    private void Start()
    {
    }

    public void SetIndex(byte index)
    {
      previousIndex = slotIndex;
      slotIndex = index;

      for (var i = 0; i < slots.Count; i++)
        slots[i].SetEnabled(i == index);

      CallSlotChanged(index);
    }

    public void AssignSlot(byte slotIdx, byte? invenIdx)
    {
      slots[slotIdx].SetIndex(invenIdx);
      CallSlotChanged();
    }

    public Item GetItem() => GetItem(slotIndex);

    [CanBeNull]
    public Item GetItem(byte slotIdx)
    {
      var idx = slots[slotIdx].invenIndex;

      return idx is null ? null : (
        inven[idx.Value].HasValue ? inven[idx.Value].Value.item : null
      );
    }

    public void CallSlotChanged() => CallSlotChanged(slotIndex);

    public void CallSlotChanged(byte idx) => onSlotChanged?.Invoke(idx);

  }
}
