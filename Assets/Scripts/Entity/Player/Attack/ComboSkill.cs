using System;
using UnityEngine;

namespace Entity.Player.Attack {
  [Serializable]
  public class ComboSkill {
    public float damagePercent = 1f;
    
    public float endTime;

    public int animParameter;

    [Header("HitBox")]
    public Vector2 hitBoxPos;

    public Vector2 hitBoxSize;

    [Header("Sound")]
    public string sound;

    public float soundDelay = 0f;
  }
}