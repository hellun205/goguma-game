using System;

namespace Entity
{
  /// <summary>
  /// 엔티티 풀 관리에 대한 데이터 입니다.
  /// </summary>
  [Serializable]
  public class EntityPoolManageData
  {
    /// <summary>
    /// 엔티티의 종류를 가져옵니다.
    /// </summary>
    public EntityType type;

    /// <summary>
    /// 엔티티의 프리팹을 가져옵니다.
    /// </summary>
    public Entity prefab;

    /// <summary>
    /// 풀의 최대 개수를 가져옵니다.
    /// </summary>
    public byte maxCount;

    public bool isUI = false;
  }
}
