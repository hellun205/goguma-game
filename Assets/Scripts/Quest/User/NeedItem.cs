using System;
using Entity.Player;
using Manager;

namespace Quest.User
{
  public class NeedItem : IRequire
  {
    public string ItemName { get; }

    public int Max { get; }

    public int Current => PlayerController.Instance.inventory.ItemCount(Managers.Item.GetObject(ItemName));

    public void Add()
    {
    }

    public NeedItem(string itemName, int count)
    {
      ItemName = itemName;
      Max = count;
    }
  }
}
