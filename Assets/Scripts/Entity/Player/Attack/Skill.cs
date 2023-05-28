using System;
using UnityEngine;

namespace Entity.Player.Attack
{
  [Serializable]
  public class Skill
  {
    public float damage;

    [Header("Z Skill")]
    public Sprite zSprite;
    public ComboSkill[] zSkill;
    public float zCoolTime;
    public float zKeepComboTime = 2f;

    [Header("X Skill")]
    public Sprite xSprite;
    public ComboSkill[] xSkill;
    public float xCoolTime;
    public float xKeepComboTime = 2f;

    public (ComboSkill[] skills, float coolTime, float keepComboTime) GetComboSkill(KeyCode key) => key switch
    {
      KeyCode.Z => (zSkill, zCoolTime, zKeepComboTime),
      KeyCode.X => (xSkill, xCoolTime, xKeepComboTime),
      _ => throw new NotImplementedException()
    };
  }
}
