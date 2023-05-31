using System;
using System.Collections;
using Entity.UI;
using Manager;
using UnityEngine;

namespace Entity
{
  /// <summary>
  /// 엔티티의 이름을 표시 하는 컴포넌트
  /// </summary>
  public class NameTag : MonoBehaviour
  {
    /// <summary>
    /// 이름표 엔티티
    /// </summary>
    private DisplayText displayText;

    /// <summary>
    /// 이름표를 표시 할 엔티티
    /// </summary>
    private Entity entity;

    [SerializeField]
    private Collider2D col;

    [SerializeField]
    private float distance = 0.1f;

    private float colDistance;

    private Vector2 GetPos() => new Vector2(entity.position.x, entity.position.y + colDistance + distance);

    private void Awake()
    {
      entity = GetComponent<Entity>();
      colDistance = col.bounds.extents.y;
      EntityManager.Instance.onGetAfter += OnGetEntity;
      EntityManager.Instance.onReleaseBefore += OnReleasedEntity;
    }

    private void Update()
    {
      displayText.text = entity.entityName;
      displayText.position = GetPos();
    }

    public void OnGetEntity(Entity entity)
    {
      if (entity == this.entity)
        displayText = Managers.Entity.GetEntity<DisplayText>(GetPos(), x => x.text = entity.entityName);
    }

    public void OnReleasedEntity(Entity entity)
    {
      if (entity == this.entity)
        displayText.Release();
    }

    private void OnDestroy()
    {
      EntityManager.Instance.onGetAfter -= OnGetEntity;
      EntityManager.Instance.onReleaseBefore -= OnReleasedEntity;
    }
  }
}
