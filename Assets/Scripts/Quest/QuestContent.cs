using UnityEngine;

namespace Quest
{
  public abstract class QuestContent : ScriptableObject
  {
    public abstract string descriptions { get; }
  }
}
