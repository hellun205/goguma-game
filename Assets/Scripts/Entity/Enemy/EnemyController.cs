using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Enemy {
  public class EnemyController : MonoBehaviour {
    public float hp;
    public float maxHp;

    private void Awake() {
      hp = maxHp;
    }

    public void Hit(float damage) {
      hp -= damage;
      Debug.Log($"hp : {hp}");
    }
  }
}