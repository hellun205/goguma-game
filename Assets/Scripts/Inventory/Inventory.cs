using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Item;
using UnityEngine;

namespace Inventory {
  public class Inventory {
    public delegate void _onItemChanged();

    public event _onItemChanged onItemChanged;

    public List<(Item item, byte count)?> items;
    public byte slotCount;

    private void AddItem(Item item, byte count = 1) {
      for (var i = 0 ; i < items.Count;i++) {
        if (!items[i].HasValue) {
          items[i] = (item, count);
          break;
        }
      }
    }

    private void SetItemCount(int index, byte count) => items[index] = (items[index].Value.item, count);

    public bool CanAddItem() {
      var emptyList = (from item_ in items
                       where !item_.HasValue
                       select 1).Sum();
      return emptyList > 0;
    }
    
    public ushort GainItem(Item item, ushort count = 1) {
      var linq = (from item_ in items
                  where item_.HasValue && item_.Value.item == item && item_.Value.count < byte.MaxValue
                  select items.IndexOf(item_)).ToArray();

      if (linq.Length > 0) {
        var plus = items[linq[0]].Value.count + count;
        if (plus <= byte.MaxValue) {
          SetItemCount(linq[0], (byte) plus);
        } else {
          SetItemCount(linq[0], byte.MaxValue);
          var left = GainItem(item, (byte) (count - byte.MaxValue));
          if (left > 0) return left;
        }
      } else {
        if (CanAddItem()) {
          if (count <= byte.MaxValue) {
            AddItem(item, (byte) count);
          } else {
            AddItem(item, byte.MaxValue);
            var left = GainItem(item, (byte) (count - byte.MaxValue));
            if (left > 0) return left;
          }
        } else {
          return count;
        }
      }

      onItemChanged?.Invoke();
      return 0;
    }

    public bool CheckItem(Item item, ushort count = 1) {
      var itemCnt = ItemCount(item);
      if (itemCnt == 0) return false;
      Debug.Log(itemCnt);
      return itemCnt >= count;
    }

    public ushort ItemCount(Item item) {
      var list = items.Where(x => x.HasValue && x.Value.item == item).ToArray();
      if (list.Length == 0) return 0;
      return (ushort) list.Sum(x => x.Value.count);
    }

    public bool LoseItem(Item item, ushort count = 1) {
      if (!CheckItem(item, count)) return false;

      var linq = (from item_ in items
                  where item_.HasValue && item_.Value.item == item
                  select items.IndexOf(item_)).ToArray();
      foreach (var i in linq) {
        var itemCount = this[i].count;
        if (count < itemCount) {
          items[i] = (item, (byte) (itemCount - count));
          break;
        }

        if (count == itemCount) {
          items[i] = null;
          break;
        }

        if (count > itemCount) {
          count -= itemCount;
          items[i] = null;
        }
      }

      onItemChanged?.Invoke();
      return true;
    }

    public void Move(byte aIdx, byte bIdx) {
      if (items[bIdx].HasValue) {
        (items[bIdx], items[aIdx]) = (items[aIdx], items[bIdx]);
      } else {
        items[bIdx] = items[aIdx];
        items[aIdx] = null;
      }
      onItemChanged?.Invoke();
    }

    public Inventory(byte slotCount) {
      this.items = new List<(Item item, byte count)?>();
      this.slotCount = slotCount;
      for (var i = 0; i < this.slotCount; i++) {
        items.Add(null);
      }
    }

    public (Item item, byte count) this[int idx] => items[idx].Value;
  }
}