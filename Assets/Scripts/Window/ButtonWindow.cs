using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Window
{
  public abstract class ButtonWindow<T> : AnimatedWindow<T> where T : ButtonWindow<T>
  {
    [Header("Button - Objects")]
    [SerializeField]
    private Button trueBtn;

    [SerializeField]
    private TextMeshProUGUI trueBtnTMP;

    [SerializeField]
    private Button falseBtn;

    [SerializeField]
    private TextMeshProUGUI falseBtnTMP;

    [SerializeField]
    private KeyCode trueKey = KeyCode.Return;

    [SerializeField]
    private KeyCode falseKey = KeyCode.Escape;
    
    [SerializeField]
    private Image btnPanel;

    public UnityEvent<bool> onBtnClick;

    [Header("Button")]
    public string falseBtnText = "확인";

    public string trueBtnText = "취소";

    private void Update()
    {
      if (!interactable) return;
      
      if (Input.GetKeyDown(trueKey))
        onBtnClick?.Invoke(true);
      if (Input.GetKeyDown(falseKey))
        onBtnClick?.Invoke(false);
    }

    protected override void Awake()
    {
      base.Awake();
      trueBtn.onClick.AddListener(() => onBtnClick?.Invoke(true));
      falseBtn.onClick.AddListener(() => onBtnClick?.Invoke(false));

      onBtnClick.AddListener(_ => OnCloseButtonClick());
    }

    protected override void OnValidate()
    {
      base.OnValidate();

      trueBtnTMP.text = trueBtnText;
      falseBtnTMP.text = falseBtnText;

      trueBtn.gameObject.SetActive(!string.IsNullOrEmpty(trueBtnText));
      falseBtn.gameObject.SetActive(!string.IsNullOrEmpty(falseBtnText));
      btnPanel.gameObject.SetActive(!string.IsNullOrEmpty(trueBtnText) || !string.IsNullOrEmpty(falseBtnText));
    }

    protected void Init(string trueText = "확인", string falseText = "취소")
    {
      this.falseBtnText = falseText;
      this.trueBtnText = trueText;

      OnValidate();
    }
  }
}
