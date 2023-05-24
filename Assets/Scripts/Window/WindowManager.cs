using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Window {
  public class WindowManager : MonoBehaviour {
    public static WindowManager Instance { get; private set; }

    public Dictionary<WindowType, IObjectPool<BaseWindow>> pools;

    [SerializeField]
    private RectTransform container;

    [SerializeField]
    private BaseWindow[] prefabs;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(this);
      pools = new Dictionary<WindowType, IObjectPool<BaseWindow>>();
      foreach (WindowType type in Enum.GetValues(typeof(WindowType))) {
        pools.Add(type, new ObjectPool<BaseWindow>(
          () => Instantiate(prefabs[(int)type], container),
          window => window.gameObject.SetActive(true),
          window => {
            window.SetDefault();
            window.gameObject.SetActive(false);
          },
          window => Destroy(window.gameObject)));
      }
    }

    public void ShowMsgBox(string title, string text, string yesText = "예", string noText = "아니요",
      UnityAction<bool> callback = null) {
      var window = pools[WindowType.MessageBox].Get();
      if (window is not MessageBoxWindow msgBox) return;
      msgBox.title = title;
      msgBox.Set(text, yesText, noText, callback);
    }

    public void ShowInputBox(string title, string text, UnityAction<string> callback, string defValue = "",
      string confirm = "확인", string cancel = "취소") {
      var window = pools[WindowType.InputBox].Get();
      if (window is not InputBoxWindow inputBox) return;
      inputBox.title = title;
      inputBox.Set(text, confirm, cancel, defValue, callback);
      InputBoxWindow.isEnabled = true;
    }

    public static void Ask(string title, string text, string yesText = "예", string noText = "아니요",
      UnityAction<bool> callback = null)
      => Instance.ShowMsgBox(title, text, yesText, noText, callback);

    public static void Show(string title, string text, string comfirmText = "확인", UnityAction callBack = null)
      => Instance.ShowMsgBox(title, text, comfirmText, string.Empty, b => callBack?.Invoke());

    public static void Read(string title, UnityAction<string> callback, string text = "", string defValue = "",
      string confirm = "확인", string cancel = "취소")
      => Instance.ShowInputBox(title, text, callback, defValue, confirm, cancel);
  }
}