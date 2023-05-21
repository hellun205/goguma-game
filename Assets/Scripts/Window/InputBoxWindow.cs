﻿using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Window {
  public class InputBoxWindow : BaseWindow {
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

    public static bool isEnabled = false;

    [CanBeNull]
    public UnityAction<string> onSubmit;
    
    public IObjectPool<InputBoxWindow> pool { private get; set; }

    protected override void OnValidate() {
      base.OnValidate();
      textTMP.text = text;
      trueBtnTMP.text = confirmBtnText;
      falseBtnTMP.text = cancelBtnText;
      inputField.text = inputText;
      placeholderTMP.text = placeholder;
    }

    protected override void Awake() {
      base.Awake();
      trueBtn.onClick.AddListener(() => onSubmit?.Invoke(inputField.text));
      trueBtn.onClick.AddListener(OnCloseButtonClick);
      falseBtn.onClick.AddListener(OnCloseButtonClick);
      Set(text, confirmBtnText, cancelBtnText);
    }

    public void Set(string text = "", string trueText = "확인", string falseText = "취소", string defValue = "", UnityAction<string> callback = null) {
      this.text = text;
      this.confirmBtnText = trueText;
      this.cancelBtnText = falseText;
      this.inputText = defValue;
      falseBtn.gameObject.SetActive(!string.IsNullOrEmpty(falseText));
      onSubmit = callback;
      OnValidate();
    }

    protected override void OnCloseButtonClick() {
      isEnabled = false;
      pool.Release(this);
    }
    
    private void Update() {
      if (Input.GetKeyDown(KeyCode.Return)) {
        onSubmit?.Invoke(inputField.text);
      }
    }
  }
}