using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Window {
  public abstract class BaseWindow : MonoBehaviour {
    [Header("UI Object - Base Window")]
    [SerializeField]
    protected TextMeshProUGUI titleTMP;
    
    [SerializeField]
    protected Button closeBtn;

    [Header("Window")]
    public string title;

    public abstract WindowType type { get; }

    protected virtual void Awake() {
      closeBtn.onClick.AddListener(OnCloseButtonClick);
    }

    protected virtual void OnCloseButtonClick() {
      WindowManager.Instance.pools[type].Release(this);
    }

    protected virtual void OnValidate() {
      titleTMP.text = title;
    }

    public abstract void SetDefault();

  }
}