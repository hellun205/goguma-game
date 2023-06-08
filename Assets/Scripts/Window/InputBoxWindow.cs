using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Window
{
  public class InputBoxWindow : ButtonWindow<InputBoxWindow>
  {
    [Header("Input Box - Objects")]
    [SerializeField]
    private TextMeshProUGUI textTMP;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TextMeshProUGUI placeholderTMP;

    [Header("Input Box")]
    public string text;

    public string inputText = "";

    public string placeholder = "Enter Text...";

    [CanBeNull]
    public UnityAction<string> onSubmit;

    protected override void OnValidate()
    {
      base.OnValidate();
    
      textTMP.text = text;
      inputField.text = inputText;
      placeholderTMP.text = placeholder;
    }

    protected override void Awake()
    {
      base.Awake();
      
      onBtnClick.AddListener(OnBtnClick);
      Init(text, falseBtnText, trueBtnText);
    }

    private void OnBtnClick(bool value)
    {
      if (value) onSubmit?.Invoke(inputField.text);
    }

    public void Init
    (
      string text = "",
      string trueText = "확인",
      string falseText = "취소",
      string defValue = "",
      UnityAction<string> callback = null
    )
    {
      this.text = text;
      this.inputText = defValue;
      onSubmit = callback;

      base.Init(trueText, falseText);
      inputField.ActivateInputField();
    }
  }
}
