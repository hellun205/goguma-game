using System;
using System.Collections.Generic;
using System.Linq;

namespace Quest.User
{
  [Serializable]
  public class QuestData
  {
    public List<QuestInfo> quests;

    public List<int> endedQuest;

    public void RemoveQuest(int questId)
    {
      quests.Remove(quests.First(x => x.questIndex == questId));
    }
  }
}
