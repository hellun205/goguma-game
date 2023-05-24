using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Enemy {
  public class EnemyController : Entity {
    public float hp;
    public float maxHp;

    public override EntityType type => EntityType.Enemy;

    private HpBar hpBar;

    protected override void Awake() {
      base.Awake();
      hpBar = GetComponent<HpBar>();
      hp = maxHp;
      hpBar.maxHp = maxHp;
      hpBar.curHp = hp;
    }

    public void Hit(float damage) {
      hp -= damage;
      Debug.Log($"hp : {hp}");
      hpBar.curHp = hp;
    }
  }
}