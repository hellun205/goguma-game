using System;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest
{
  [Serializable]
  public struct Reward
  {
    public RewardType type;

    public string itemName;

    [Min(1)]
    public ushort amount;

    public bool CanCompensate()
    {
      var player = Managers.Player;
      switch (type)
      {
        case RewardType.Item:
        {
          var item = Managers.Item.GetObject(itemName);
          return player.inventory.CanGainItem(item, amount);
        }
        default:
          return true;
      }
    }

    public void Compensate()
    {
      var player = Managers.Player;
      switch (type)
      {
        case RewardType.Item:
        {
          var item = Managers.Item.GetObject(itemName);
          player.inventory.GainItem(item, amount);
          break;
        }

        case RewardType.Exp:
        {
          player.status.exp += amount;
          break;
        }

        case RewardType.Level:
        {
          player.status.level += (byte) amount;
          break;
        }
      }
    }
  }
}
