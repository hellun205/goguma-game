using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Entity.UI {
  /// <summary>
  /// DisplayText 엔티티의 컴포넌트
  /// </summary>
  public class DisplayText : UIEntity {
    public override EntityType type => EntityType.DisplayText;

    /// <summary>
    /// 내용을 표시 할 TextmeshProUGUI 컴포넌트를 가져옵니다.
    /// </summary>
    [FormerlySerializedAs("text")]
    [SerializeField]
    private TextMeshProUGUI textTMP;

    /// <summary>
    /// 배경을 표시 할 Image 컴포넌트를 가져옵니다.
    /// </summary>
    [SerializeField]
    private Image img;
    
    private byte _line;
    
    /// <summary>
    /// 넓이를 지정합니다.
    /// </summary>
    public float width {
      get => rectTransform.sizeDelta.x;
      set => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
      }

    /// <summary>
    /// 줄(길이)을 지정합니다.
    /// </summary>
    public byte line {
      get => _line;
      set {
        _line = value;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _line * 20f + 20f);
      }
    }

    /// <summary>
    /// 표시할 내용을 지정합니다.
    /// </summary>
    public string text {
      get => textTMP.text;
      set => textTMP.text = value;
    }

    protected override void Awake() {
      base.Awake();

      width = 300f;
      line = 1;
    }

    // private void OnBecameInvisible() => pool.Release(this);
  }
}