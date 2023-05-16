using System;
using UnityEngine;

namespace Player.Attack {
  [Serializable]
  public class Skill {
    public SkillType type;

    public float damagePercent = 1f;

    [Header("Cooldown")]
    public float coolTime;

    public float endTime;

    [Header("HitBox")]
    public Transform hitBoxPos;

    public Vector2 hitBoxSize;

    [Header("Sound")]
    public AudioClip sound;

    public float soundDelay = 0f;
  }
}