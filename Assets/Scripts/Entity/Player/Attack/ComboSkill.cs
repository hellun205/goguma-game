using System;
using Audio;
using UnityEngine;

namespace Entity.Player.Attack
{
  [Serializable]
  public class ComboSkill
  {
    [Range(0.1f, 5f)]
    public float damagePercent = 1f;

    [Range(0.1f, 3f)]
    public float endTime;
    
    public int animParameter;

    public string audio;

    [Header("HitBox")]
    public Vector2 hitBoxPos;

    public Vector2 hitBoxSize;
  }
}
