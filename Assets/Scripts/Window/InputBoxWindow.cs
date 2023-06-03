using JetBrains.Annotations;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Window
{
  public class InputBoxWindow : BaseWindow
  {
    [Header("UI Object - Input Box")]
    [SerializeField]
    private TextMeshProUGUI textTMP;

    [SerializeField]
    private Button trueBtn;

    [SerializeField]
    private TextMeshProUGUI trueBtnTMP;

    [SerializeField]
    private Button falseBtn;

    [SerializeField]
    private TextMeshProUGUI falseBtnTMP;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TextMeshProUGUI placeholderTMP;

    [Header("Input Box")]
    public string text;

    public string confirmBtnText = "확인";

    public string cancelBtnText = "취소";

    public string inputText = "";

    public string placeholder = "Enter Text...";

    [CanBeNull]
    public UnityAction<string> onSubmit;

    protected override void OnCloseButtonClick()
      => Managers.Window.Release(this);

    protected override void OnValidate()
    {
      base.OnValidate();

      textTMP.text = text;
      trueBtnTMP.text = confirmBtnText;
      falseBtnTMP.text = cancelBtnText;
      inputField.text = inputText;
      placeholderTMP.text = placeholder;
    }

    protected override void Awake()
    {
      base.Awake();

      trueBtn.onClick.AddListener(() => onSubmit?.Invoke(inputField.text));
      trueBtn.onClick.AddListener(OnCloseButtonClick);
      falseBtn.onClick.AddListener(OnCloseButtonClick);

      Init(text, confirmBtnText, cancelBtnText);
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
      this.confirmBtnText = trueText;
      this.cancelBtnText = falseText;
      this.inputText = defValue;
      onSubmit = callback;

      falseBtn.gameObject.SetActive(!string.IsNullOrEmpty(falseText));
      OnValidate();
      inputField.ActivateInputField();
    }

    private void Update()
    {
      if (!Input.GetKeyDown(KeyCode.Return))
        return;

      onSubmit?.Invoke(inputField.text);
      OnCloseButtonClick();
    }
  }
}
