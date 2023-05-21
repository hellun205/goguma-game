using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Window {
  public class BaseWindow : MonoBehaviour {
    [Header("UI Object - Base Window")]
    [SerializeField]
    protected TextMeshProUGUI titleTMP;
    
    [SerializeField]
    protected Button closeBtn;

    [Header("Window")]
    public string title;

    protected virtual void Awake() {
      closeBtn.onClick.AddListener(OnCloseButtonClick);
    }

    protected virtual void OnCloseButtonClick() {
      Destroy(gameObject);
    }

    protected virtual void OnValidate() {
      titleTMP.text = title;
    }

  }
}