using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Entity.UI
{
  /// <summary>
  /// DisplayText 엔티티의 컴포넌트
  /// </summary>
  public class DisplayText : UIEntity
  {
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

    /// <summary>
    /// 표시할 내용을 지정합니다.
    /// </summary>
    public string text
    {
      get => textTMP.text;
      set => textTMP.text = value;
    }

    protected override void Awake()
    {
      base.Awake();
    }
  }
}
