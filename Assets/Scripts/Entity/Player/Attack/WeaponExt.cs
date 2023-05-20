using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Attack {
  public static class WeaponExt {
    public static Weapon Get(this IEnumerable<Weapon> list, Weapons type) =>
      list.Where(weapon => weapon.type == type).ToArray()[0];

    public static Skill Get(this IEnumerable<Skill> list, SkillType type) =>
      list.Where(skill => skill.type == type).ToArray()[0];
    
    public static KeyCode GetKey(this SkillType skillType) => skillType switch {
      SkillType.Z => KeyCode.Z,
      SkillType.X => KeyCode.X,
      _ => throw new NotImplementedException()
    };

    public static SkillType GetSkill(this KeyCode key) => key switch {
      KeyCode.Z => SkillType.Z,
      KeyCode.X => SkillType.X,
      _ => throw new NotImplementedException()
    };
  }
}