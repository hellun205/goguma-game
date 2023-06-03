using System.Collections.Generic;
using Animation.Preset;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
  public class InventoryManager : SingleTon<InventoryManager>
  {
    public bool activeInventory { get; private set; } = false;

    private Inventory _inventory;

    public Inventory inventory
    {
      get => _inventory;
      set
      {
        if (_inventory is not null) 
          _inventory.onItemChanged -= Refresh;
        _inventory = value;
        SetCount(value.slotCount);
        _inventory.onItemChanged += Refresh;
      }
    }

    [Header("UI Object")]
    [SerializeField]
    private Transform content;

    [Header("Slot")]
    private Slot slotPrefab;

    [SerializeField]
    private byte slotCount = 28;

    private List<Slot> slots = new List<Slot>();

    public const byte horizontalCount = 4;

    public Image dragImg;

    [HideInInspector]
    public byte dragedIdx;

    [HideInInspector]
    public bool isDragging;

    [Header("ToolTip")]
    public ItemToolTip toolTipPanel;

    private PanelVisibler anim;

    protected override void Awake()
    {
      base.Awake();
      slotPrefab = PrefabManager.Instance.GetObject<Slot>("InventorySlot");
      toolTipPanel.gameObject.SetActive(false);
      anim = new(this);

      SetCount(slotCount);
    }

    private void Update()
    {
      if (Input.GetKeyDown(Managers.Key.openInventory))
        ToggleActive();
    }

    private void ToggleActive()
    {
      activeInventory = !activeInventory;
      Refresh();
      if (activeInventory)
        anim.Show();
      else
        anim.Hide();
      
      toolTipPanel.gameObject.SetActive(false);
      Managers.Audio.PlaySFX("open_inventory");
    }

    public void SetCount(byte count)
    {
      slotCount = count;
      ClearSlot();

      for (var i = 0; i < slotCount; i++)
      {
        var slot = Instantiate(slotPrefab, content);
        slot.index = (byte)i;
        slots.Add(slot);
      }
    }

    private void ClearSlot()
    {
      if (slots.Count == 0)
        return;

      foreach (var slot in slots)
        Destroy(slot.gameObject);

      slots.Clear();
    }

    public void Refresh()
    {
      for (var i = 0; i < inventory.items.Count; i++)
      {
        var item = inventory.items[i];

        if (item.HasValue)
          slots[i].SetItem(item.Value.item, item.Value.count);
        else
          slots[i].SetItem();
      }
    }
  }
}
