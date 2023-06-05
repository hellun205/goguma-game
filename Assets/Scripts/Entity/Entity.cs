using Manager;
using UnityEngine;

namespace Entity
{
  public abstract class Entity : MonoBehaviour
  {
    public string entityName;

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
    
    public virtual void Release() => Managers.Entity.ReleaseEntity(this);

    public virtual void OnGet() => onGet?.Invoke(this);

    public virtual void OnRelease() => onRelease?.Invoke(this);

    private void ThrowItemB(Item.BaseItem item, byte count, sbyte direction = 1)
    {
      var startPositionX = (position.x + (col.bounds.extents.x + 0.6f) * direction);
      var pos = new Vector2(startPositionX, position.y);
      // var throwItem = Entity.SummonItem(new Vector2(startPositionX, position.y), item, count);
      var throwItem = Managers.Entity.GetEntity<Item.EItem>(pos, x => x.Init(item, count));
      
      throwItem.Throw(new Vector2(direction * 2f, 3f), 4f);
    }

    public void ThrowItem(Item.BaseItem item, ushort count, sbyte direction = 1)
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
