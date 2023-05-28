using Entity.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity
{
  /// <summary>
  /// 기본적인 엔티티의 기능을 가진 컴포넌트 입니다.
  /// </summary>
  public abstract partial class Entity : MonoBehaviour
  {
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
    public virtual Vector2 position
    {
      get => transform.position;
      set => transform.position = value;
    }

    public bool canDespawn = true;

    public event EntityManager.entityEvent onGet;
    public event EntityManager.entityEvent onRelease;

    [SerializeField]
    protected BoxCollider2D col;

    protected virtual void OnBecameInvisible()
    {
      if (canDespawn)
        Release();
    }

    public void Translate(float x, float y) => position = new Vector2(position.x + x, position.y + y);

    /// <summary>
    /// 엔티티를 삭제합니다.
    /// </summary>
    public virtual void Release() => EntityManager.Release(this);

    public virtual void OnGet() => onGet?.Invoke(this);

    public virtual void OnRelease() => onRelease?.Invoke(this);

    private void ThrowItemB(Item.Item item, byte count, sbyte direction = 1)
    {
      var startPositionX = (position.x + (col.bounds.extents.x + 0.6f) * direction);
      var throwItem = Entity.SummonItem(new Vector2(startPositionX, position.y), item, count);

      throwItem.Throw(new Vector2(direction * 2f, 3f), 4f);
    }

    public void ThrowItem(Item.Item item, ushort count, sbyte direction = 1)
    {
      if (count <= byte.MaxValue)
        ThrowItemB(item, (byte)count, direction);
      else
      {
        ThrowItemB(item, byte.MaxValue, direction);
        ThrowItem(item, (ushort)(count - byte.MaxValue), direction);
      }
    }
  }
}
