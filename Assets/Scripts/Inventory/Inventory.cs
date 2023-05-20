﻿using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Item;
using UnityEngine;

namespace Inventory {
  public class Inventory {
    public List<(Item item, byte count)> items;
    public byte slotCount;

    private void AddItem(Item item, byte count = 1) => items.Add((item, count));

    private void SetItemCount(int index, byte count) => items[index] = (items[index].item, count);

    public byte GainItem(Item item, byte count = 1) {
      var linq = (from item_ in items
                  where item_.item == item && item_.count < byte.MaxValue
                  select items.IndexOf(item_)).ToArray();
      
      if (linq.Length > 0) {
        var plus = items[linq[0]].count + count;
        if (plus <= byte.MaxValue) {
          SetItemCount(linq[0], (byte)plus);
        } else {
          SetItemCount(linq[0], byte.MaxValue);
          var left = GainItem(item, (byte)(count - byte.MaxValue));
          if (left > 0) return left;
        }
      } else {
        if (items.Count < slotCount) {
          if (count <= byte.MaxValue) {
            AddItem(item, count);
          } else {
            AddItem(item, byte.MaxValue);
            var left = GainItem(item, (byte)(count - byte.MaxValue));
            if (left > 0) return left;
          }
        } else {
          return count;
        }
      }

      return 0;
    }

    public Inventory(byte slotCount) {
      this.items = new List<(Item item, byte count)>();
      this.slotCount = slotCount;
    }
  }
}