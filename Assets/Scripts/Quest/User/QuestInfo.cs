using System;
using System.Collections.Generic;
using System.Linq;

namespace Quest.User
{
  [Serializable]
  public class QuestInfo
  {
    public int questIndex;
    
    public List<IRequire> requires = new List<IRequire>();

    public bool isCompleted => requires.All(require => require.Max <= require.Current);
  }
}
