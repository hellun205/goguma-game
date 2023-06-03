﻿using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Window
{
  public class MessageBoxWindow : BaseWindow
  {

    [Header("UI Object - Message Box")]
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
    private Image btnPanel;

    [Header("Message Box")]
    public string text;

    public string trueBtnText = "예";

    public string falseBtnText = "아니오";

    public UnityEvent<bool> onBtnClick;

    protected override void OnCloseButtonClick()
      => Managers.Window.Release(this);

    protected override void OnValidate()
    {
      base.OnValidate();
      textTMP.text = text;
      trueBtnTMP.text = trueBtnText;
      falseBtnTMP.text = falseBtnText;
    }

    protected override void Awake()
    {
      base.Awake();
      trueBtn.onClick.AddListener(() => onBtnClick?.Invoke(true));
      falseBtn.onClick.AddListener(() => onBtnClick?.Invoke(false));
      Init(text, trueBtnText, falseBtnText);
    }

    public void Init(string text = "", string trueText = "예", string falseText = "아니오", UnityAction<bool> onClick = null)
    {
      this.text = text;
      this.trueBtnText = trueText;
      this.falseBtnText = falseText;

      btnPanel.gameObject.SetActive(!string.IsNullOrEmpty(trueText) || !string.IsNullOrEmpty(falseText));
      trueBtn.gameObject.SetActive(!string.IsNullOrEmpty(trueText));
      falseBtn.gameObject.SetActive(!string.IsNullOrEmpty(falseText));

      onBtnClick.RemoveAllListeners();
      onBtnClick.AddListener(_ => OnCloseButtonClick());

      if (onClick != null)
        onBtnClick.AddListener(onClick);

      OnValidate();
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Return))
        onBtnClick?.Invoke(true);
    }
  }
}