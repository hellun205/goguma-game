using Animation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.UI
{
  public class UEHpBar : UIEntity
  {
    private const float middleFollowSpeed = 4f;

    private SmoothFloat animMiddle;

    [SerializeField]
    private Image topImg;

    [SerializeField]
    private Image middleImg;

    private float _value;
    private float _maxValue;
    private bool _showText;

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
      animMiddle = new(this, new(() => middleImg.fillAmount, value => middleImg.fillAmount = value));
    }

    public void Init(float value, float maxValue)
    {
      this.value = value;
      this.maxValue = maxValue;
    }
  }
}
