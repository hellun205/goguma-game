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

    /// <summary>
    /// 이름표의 위치
    /// </summary>
    [SerializeField]
    private Transform position;

    private void Awake() {
      entity = GetComponent<Entity>();
      // Init();
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
      displayText.text = entity.entityName;
      displayText.position = position.position;
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