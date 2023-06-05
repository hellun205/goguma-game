using Entity.Enemy;
using Entity.Item;
using Entity.Player;
using Manager;
using UnityEngine;

namespace Quest
{
  [CreateAssetMenu(fileName = "Kill Enemy", menuName = "Quest/Contents/Kill Enemy", order = 0)]
  public class QKillEnemy : QuestContent
  {
    public string enemyName;
  
    [Min(1)]
    public ushort count = 1;

    private EnemyController _enemy;

    private EnemyController enemy => _enemy ??= Managers.Prefab.GetObject<EnemyController>(enemyName);
  
    public override string descriptions => $"{enemy.entityName} {count}마리 처치";
  }
}
