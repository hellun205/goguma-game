using System;
using System.Collections.Generic;
using Entity.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.QuickSlot {
  public class QuickSlotController : MonoBehaviour {
    private List<QuickSlot> slots = new List<QuickSlot>();

    public Inventory inven => PlayerController.Instance.inventory;

    public byte slotIndex { get; private set; }

    private void Awake() {
      slots.AddRange(GetComponentsInChildren<QuickSlot>());
      for (var i = 0; i < slots.Count; i++) {
        slots[i].index = (byte)i;
      }
    }

    private void Start() {
    }

    public void SetIndex(byte index) {
      slotIndex = index;
      for (var i = 0; i < slots.Count; i++) {
        slots[i].SetEnabled(i == index);
      }
    }

    public void AssignSlot(byte slotIdx, byte invenIdx) {
      slots[slotIdx].SetIndex(invenIdx);
    }
    
  }
}