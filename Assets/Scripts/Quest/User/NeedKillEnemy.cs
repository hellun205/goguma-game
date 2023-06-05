using UnityEngine;

namespace Quest.User
{
  public class NeedKillEnemy : IRequire
  {
    public string EnemyName { get; }
    
    public int Max { get; }

    public int Current { get; private set; }

    public void Add()
    {
      Current++;
      Debug.Log($"{Current} / {Max}");
    }

    public NeedKillEnemy(string enemyName, int count)
    {
      EnemyName = enemyName;
      Max = count;
      Current = 0;
    }
  }
}
