using Animation;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.UI
{
  public class HealthBar : UIEntity
  {
    public static EntityType Type => EntityType.HpBar; 
    public override EntityType type => Type;

    private const float middleFollowSpeed = 4f;

    private SmoothFloat animMiddle;

    [SerializeField]
    private Image topImg;

    [SerializeField]
    private Image middleImg;

    private float _value;
    private float _maxValue;

    public float value
    {
      get => _value;
      set
      {
        _value = value;
        topImg.fillAmount = _value / maxValue;
        animMiddle.Start(middleImg.fillAmount, topImg.fillAmount, middleFollowSpeed);
      }
    }

    public float maxValue
    {
      get => _maxValue;
      set
      {
        _maxValue = value;
        this.value = this.value;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      animMiddle = new SmoothFloat(this, 1f, value => middleImg.fillAmount = value);
    }

    // protected void Update()
    // {
    //   yellowSlider.value = Mathf.Lerp(
    //     yellowSlider.value,
    //     redSlider.value,
    //     Time.deltaTime * smoothing
    //   );
    // }

    public void Init(float value, float maxValue)
    {
      this.value = value;
      this.maxValue = maxValue;
    }
  }
}
