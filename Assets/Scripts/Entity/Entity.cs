using UnityEngine;
using UnityEngine.Serialization;

namespace Entity {
  /// <summary>
  /// 기본적인 엔티티의 기능을 가진 클래스 입니다.
  /// </summary>
  public abstract class Entity : MonoBehaviour {
    /// <summary>
    /// 엔티티의 이름을 가져옵니다.
    /// </summary>
    [FormerlySerializedAs("name")]
    public string entityName;
    
    /// <summary>
    /// 엔티티의 종류를 가져옵니다.
    /// </summary>
    public abstract EntityType type { get; }

    /// <summary>
    /// 엔티티의 위치 좌표를 지정하거나 가져옵니다.
    /// </summary>
    public virtual Vector3 position {
      get => transform.position;
      set => transform.position = value;
    }

    // private void OnBecameInvisible() => Release();

    /// <summary>
    /// 엔티티를 삭제합니다.
    /// </summary>
    public void Release() => EntityManager.Release(this);
    
  }
}