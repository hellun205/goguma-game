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

    private void AddItemOnList(BaseItem item, byte count = 1)
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
        .Where(x => x.HasValue && x.Value.item == item)
        .Sum(x => byte.MaxValue - x.Value.count);
      var empties = items
        .Where(x => !x.HasValue)
        .Sum(_ => byte.MaxValue);

      return (ushort)(equals + empties);
    }

    public ushort GainItem(BaseItem item, ushort count = 1)
    {
      var i = 0;
      while (count > 0)
      {
        var equalIndexes = items
          .Where(x => x.HasValue && x.Value.item == item && x.Value.count < byte.MaxValue)
          .Select(x => items.IndexOf(x.Value))
          .ToArray();
        if (!equalIndexes.Any())
        {
          if (items.All(x => x.HasValue))
            break;
          var c = CalcCount(0, count, out var left);
          AddItemOnList(item, c);
          count = left;
        }
        else
        {
          var it = items[equalIndexes[i]].Value;
          
          if (it.count < byte.MaxValue)
          {
            var c = CalcCount(it.count, count, out var left);
            SetItemCount(equalIndexes[i], c);
            count = left;
          }
          else 
            i++;
        }
      }
      
      onItemChanged?.Invoke();
      return count;
    }

    public bool LoseItem(BaseItem item, ushort count = 1)
    {
      

      onItemChanged?.Invoke();
      return true;
    }

    private static byte CalcCount(byte curCount, ushort countToAdd, out ushort leftCount)
    {
      var addable = byte.MaxValue - curCount;
      
      if (countToAdd >= addable)
      {
        leftCount = (ushort)(countToAdd - addable);
        return byte.MaxValue;
      }
      else
      {
        leftCount = 0;
        return (byte)(curCount + countToAdd);
      }
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

      return (ushort)list.Sum(x => x.Value.count);
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