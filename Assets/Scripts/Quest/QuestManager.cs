using System;
using System.Linq;
using Manager;
using UnityEngine;

namespace Quest
{
  public class QuestManager : ObjectCollector<QuestManager, Quest>
  {
    public int GetQuestID(Quest quest)
    {
      var q = items.SingleOrDefault(q => q == quest);
      if (q is null) throw new Exception($"Can't find quest in list: {quest._name}");
      return q.index;
    }

    public Quest GetQuestByID(int id)
    {
      var q = items.SingleOrDefault(q => q.index == id);
      if (q is null) throw new Exception($"Can't find quest by id: {id}");
      return q;
    }
  }
}
