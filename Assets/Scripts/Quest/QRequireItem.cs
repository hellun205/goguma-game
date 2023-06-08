using Entity.Item;
using Manager;
using UnityEngine;

namespace Quest
{
  [CreateAssetMenu(fileName = "Require Item", menuName = "Quest/Contents/Require Item", order = 0)]
  public class QRequireItem : QuestContent
  {
    public string itemName;

    [Min(1)] 
    public ushort count = 1;

    private BaseItem _item;

    private BaseItem item => _item ??= Managers.Item.GetObject(itemName);

    public override string descriptions => $"{item._name} {count}개 획득";
  }
}
