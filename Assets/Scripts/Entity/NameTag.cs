using System;
using System.Collections;
using Entity.UI;
using UnityEngine;

namespace Entity {
  /// <summary>
  /// 엔티티의 이름을 표시 하는 컴포넌트
  /// </summary>
  public class NameTag : MonoBehaviour {

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

    private void Awake() {
      entity = GetComponent<Entity>();
      colDistance = col.bounds.extents.y;
      EntityManager.Instance.onGetAfter += OnGetEntity;
      EntityManager.Instance.onReleaseBefore += OnReleasedEntity;
    }

    private void Update() {
      Refresh();
    }

    /// <summary>
    /// 새로고침 합니다.
    /// </summary>
    private void Refresh() {
      var pos = entity.position;
      displayText.text = entity.entityName;
      displayText.position = new Vector3(pos.x, pos.y + colDistance+ distance);;
    }

    private void Init() {
      displayText = (DisplayText) EntityManager.Get(EntityType.DisplayText);
      Refresh();
    }

    public void OnGetEntity(Entity entity) {
      if (entity == this.entity) {
        Init();
      }
    }

    public void OnReleasedEntity(Entity entity) {
      if (entity == this.entity) {
        displayText.Release();
      }
    }

  }
}