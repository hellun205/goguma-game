using System;
using Entity.UI;
using UnityEngine;

namespace Entity {
  public class HpBar : MonoBehaviour {
    private Entity entity;
    private HealthBar hpBar;

    public float curHp;
    public float maxHp;

    [SerializeField]
    private Collider2D col;

    [SerializeField]
    private float distance= 0.1f;

    private float colDistance;

    private void Awake() {
      entity = GetComponent<Entity>();
      colDistance = col.bounds.extents.y;
      EntityManager.Instance.onGetAfter += OnGetEntity;
      EntityManager.Instance.onReleaseBefore += OnReleasedEntity;
    }

    private void Update() {
      var pos = entity.position;
      hpBar.position = new Vector3(pos.x, pos.y + colDistance+ distance);
      hpBar.maxValue = maxHp;
      hpBar.value = curHp;
    }
    
    
    public void OnGetEntity(Entity entity) {
      if (entity == this.entity) {
        LoadHpBar();
      }
    }

    public void OnReleasedEntity(Entity entity) {
      if (entity == this.entity) {
        hpBar.Release();
      }
    }

    public void LoadHpBar() {
      hpBar = (HealthBar) EntityManager.Get(EntityType.HpBar);
    }
  }
}