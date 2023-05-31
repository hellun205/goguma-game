using UnityEngine;
using UnityEngine.UI;

namespace Entity.UI
{
  public class HealthBar : UIEntity
  {
    public static EntityType Type => EntityType.HpBar; 
    public override EntityType type => Type;

    private const float smoothing = 2f;

    [SerializeField]
    private Slider redSlider;

    [SerializeField]
    private Slider yellowSlider;

    public float value
    {
      get => redSlider.value;
      set => redSlider.value = value;
    }

    public float maxValue
    {
      get => redSlider.maxValue;
      set
      {
        redSlider.maxValue = value;
        yellowSlider.maxValue = value;
      }
    }

    protected void Update()
    {
      yellowSlider.value = Mathf.Lerp(
        yellowSlider.value,
        redSlider.value,
        Time.deltaTime * smoothing
      );
    }

    public void Init(float value, float maxValue)
    {
      this.value = value;
      this.maxValue = maxValue;
    }
  }
}
