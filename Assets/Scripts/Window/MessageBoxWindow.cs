using JetBrains.Annotations;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Window
{
  public class MessageBoxWindow : ButtonWindow<MessageBoxWindow>
  {
    [Header("Message Box - Objects")]
    [SerializeField]
    private TextMeshProUGUI textTMP;

    [Header("Message Box")]
    public string text;

    [CanBeNull]
    public UnityAction<bool> onSubmit;

    protected override void OnValidate()
    {
      base.OnValidate();
      textTMP.text = text;
    }

    protected override void Awake()
    {
      base.Awake();
      onBtnClick.AddListener(b => onSubmit?.Invoke(b));
      Init(text, trueBtnText, falseBtnText);
    }

    public void Init
    (
      string text = "",
      string trueText = "예",
      string falseText = "아니오",
      UnityAction<bool> onClick = null
    )
    {
      this.text = text;
      onSubmit = onClick;

      base.Init(trueText, falseText);
    }
  }
}
