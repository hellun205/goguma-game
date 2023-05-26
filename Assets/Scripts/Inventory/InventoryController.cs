using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory {
  public class InventoryController : MonoBehaviour {
    public static InventoryController Instance { get; private set; }

    public KeyCode openKey = KeyCode.I;

    public bool activeInventory { get; private set; } = false;

    private Inventory _inventory;

    public Inventory inventory {
      get => _inventory;
      set {
        _inventory = value;
        SetCount(value.slotCount);
        _inventory.onItemChanged += Refresh;
      }
    }

    [Header("UI Object")]
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private Transform content;

    [Header("Slot")]
    [SerializeField]
    private Slot slotPrefab;

    [SerializeField]
    private byte slotCount = 28;

    private List<Slot> slots = new List<Slot>();

    public const byte horizontalCount = 4;

    public Image dragImg;

    [HideInInspector]
    public byte dragedIdx;

    [FormerlySerializedAs("isDraging")]
    [HideInInspector]
    public bool isDragging;

    [Header("ToolTip")]
    public ItemToolTip toolTipPanel;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(this);
      // DontDestroyOnLoad(gameObject);
      toolTipPanel.gameObject.SetActive(false);
      panel.SetActive(activeInventory);
      SetCount(slotCount);
    }

    private void Update() {
      if (Input.GetKeyDown(openKey)) {
        ToggleActive();
      }
    }

    private void ToggleActive() {
      activeInventory = !activeInventory;
      Refresh();
      panel.SetActive(activeInventory);
    }

    public void SetCount(byte count) {
      slotCount = count;
      ClearSlot();

      for (var i = 0; i < slotCount; i++) {
        var slot = Instantiate(slotPrefab, content);
        slot.index = (byte)i;
        slots.Add(slot);
      }
    }

    private void ClearSlot() {
      if (slots.Count == 0) return;
      foreach (var slot in slots) {
        Destroy(slot.gameObject);
      }

      slots.Clear();
    }

    public void Refresh() {
      for (var i = 0; i < inventory.items.Count; i++) {
        var item = inventory.items[i];
        if (item.HasValue)
          slots[i].SetItem(item.Value.item, item.Value.count);
        else
          slots[i].SetItem();
      }
    }
  }
}