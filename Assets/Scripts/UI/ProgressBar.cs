using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class ProgressBar : MonoBehaviour
  {
    [Header("UI Object")]
    [SerializeField]
    private Image progressImg;

    private float _maxValue;

    public float maxValue
    {
      get => _maxValue;
      set
      {
        _maxValue = value;
        this.value = this.value;
      }
    }

    public float value
    {
      get => progressImg.fillAmount * maxValue;
      set => progressImg.fillAmount = value / maxValue;
    }
  }
}
