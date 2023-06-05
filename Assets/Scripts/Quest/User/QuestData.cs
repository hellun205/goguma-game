using System;
using System.Collections.Generic;

namespace Quest.User
{
  [Serializable]
  public class QuestData
  {
    public List<QuestInfo> quests;

    public List<int> endedQuest;
  }
}
