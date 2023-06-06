using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Item;
using Entity.Npc;
using UnityEngine;

namespace Inventory
{
  public sealed class Inventory
  {
    public delegate void _onItemChanged();

    public event _onItemChanged onItemChanged;

    public List<(BaseItem item, byte count)?> items;
    public byte slotCount;

    private Inventory()
    {
      onItemChanged += ENpc.RefreshQuestAll;
    }

    private void AddItem(BaseItem item, byte count = 1)
    {
      for (var i = 0; i < items.Count; i++)
      {
        if (!items[i].HasValue)
        {
          items[i] = (item, count);
          break;
        }
      }
    }

    private void SetItemCount(int index, byte count) => items[index] = (items[index].Value.item, count);

    public bool CanAddItem()
    {
      return items.Count(item => !item.HasValue) > 0;
    }

    public bool CanGainItem(BaseItem item, ushort count) => count <= CanGainItemCount(item);

    public ushort CanGainItemCount(BaseItem item)
    {
      var equals = items
       .Where(_item => _item.HasValue && _item.Value.item == item)
       .Sum(_item => byte.MaxValue - _item.Value.count);
      var empties = items
       .Where(_item => _item.HasValue)
       .Sum(_ => byte.MaxValue);

      return (ushort) (equals + empties);
    }

    public ushort GainItem(BaseItem item, ushort count = 1)
    {
      var equalIndexes = items
       .Where((_item, _) => _item.HasValue && _item.Value.item == item)
       .Select(_item => items.IndexOf(_item.Value))
       .ToArray();

      var i = 0;
      while (count > 0)
      {
        if (i < equalIndexes.Length)
        {
          var it = items[equalIndexes[i]].Value;
          if (it.count < byte.MaxValue)
          {
            if (count > byte.MaxValue)
            {
              // Todo
            }
            else
            {
              
            }
          }
          else
            i++;
        }
        // else if ()
        // {
        // }
      }


      onItemChanged?.Invoke();
      return 0;
    }

    public bool CheckItem(BaseItem item, ushort count = 1)
    {
      var itemCnt = ItemCount(item);

      if (itemCnt == 0)
        return false;

      Debug.Log(itemCnt);

      return itemCnt >= count;
    }

    public ushort ItemCount(BaseItem item)
    {
      var list = items.Where(x => x.HasValue && x.Value.item == item).ToArray();

      if (list.Length == 0)
        return 0;

      return (ushort) list.Sum(x => x.Value.count);
    }

    public bool LoseItem(BaseItem item, ushort count = 1)
    {
      if (!CheckItem(item, count))
        return false;

      var linq = (
        from item_ in items
        where item_.HasValue && item_.Value.item == item
        select items.IndexOf(item_)
      ).ToArray();

      foreach (var i in linq)
      {
        if (!this[i].HasValue)
          continue;

        var itemCount = this[i].Value.count;

        if (count < itemCount)
        {
          items[i] = (item, (byte) (itemCount - count));
          break;
        }
        else if (count == itemCount)
        {
          items[i] = null;
          break;
        }
        else
        {
          count -= itemCount;
          items[i] = null;
        }
      }

      onItemChanged?.Invoke();
      return true;
    }

    public void Move(byte aIdx, byte bIdx)
    {
      if (items[bIdx].HasValue)
        (items[bIdx], items[aIdx]) = (items[aIdx], items[bIdx]);
      else
      {
        items[bIdx] = items[aIdx];
        items[aIdx] = null;
      }

      onItemChanged?.Invoke();
    }

    public Inventory(byte slotCount) : this()
    {
      this.items = new List<(BaseItem item, byte count)?>();
      this.slotCount = slotCount;

      for (var i = 0; i < this.slotCount; i++)
        items.Add(null);
    }

    public (BaseItem item, byte count)? this[int idx] => items[idx];
  }
}
