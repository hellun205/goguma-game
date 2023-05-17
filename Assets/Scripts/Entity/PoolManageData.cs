using System;

namespace Entity {
  [Serializable]
  public class PoolManageData {
    public EntityType type;

    public Entity prefab;

    public byte maxCount;
  }
}