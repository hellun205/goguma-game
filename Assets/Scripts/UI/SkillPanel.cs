using UnityEngine;

namespace UI {
  public class SkillPanel : MonoBehaviour {
    public SkillUI z;
    public SkillUI x;

    public Sprite noneSprite;

    public void SetCooldown(float value, float? maxValue = null) {
      z.cooldown.value = value;
      x.cooldown.value = value;

      if (maxValue is null) return;
      z.cooldown.maxValue = maxValue.Value;
      x.cooldown.maxValue = maxValue.Value;
    }
    
    public void SetActive(float value, float? maxValue = null) {
      z.active.value = value;
      x.active.value = value;

      if (maxValue is null) return;
      z.active.maxValue = maxValue.Value;
      x.active.maxValue = maxValue.Value;
    }
  }
}