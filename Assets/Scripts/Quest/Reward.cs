using System;
using Manager;

namespace Quest
{
  [Serializable]
  public struct Reward
  {
    public RewardType type;

    public string item_name;

    public ushort amount;

    public bool Compensate()
    {
      var player = Managers.Player;
      switch (type)
      {
        case RewardType.Item:
        {
          var item = Managers.Item.GetObject(item_name);
          if (!player.inventory.CanGainItem(item, amount))
            return false;

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

      return true;
    }
  }
}
