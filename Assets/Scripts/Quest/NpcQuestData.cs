using System;

namespace Quest
{
  [Serializable]
  public struct NpcQuestData
  {
    public int questID;

    public int[] requireQuests;

    public byte requireLevel;
  }
}
