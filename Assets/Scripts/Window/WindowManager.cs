using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Window {
  public class WindowManager : MonoBehaviour {
    public static WindowManager Instance { get; private set; }

    private IObjectPool<MessageBoxWindow> msgBoxPool;
    private IObjectPool<InputBoxWindow> inputBoxPool;

    [FormerlySerializedAs("prefab")]
    [SerializeField]
    private MessageBoxWindow msgbox;

    [SerializeField]
    private InputBoxWindow inputBox;

    [SerializeField]
    private RectTransform container;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(this);

      msgBoxPool = new ObjectPool<MessageBoxWindow>(
        () => {
          var window = Instantiate(msgbox, container);
          window.pool = msgBoxPool;
          return window;
        }, window => window.gameObject.SetActive(true),
        window => {
          window.Set();
          window.gameObject.SetActive(false);
        },
        window => Destroy(window.gameObject));

      inputBoxPool = new ObjectPool<InputBoxWindow>(
        () => {
          var window = Instantiate(inputBox, container);
          window.pool = inputBoxPool;
          return window;
        }, window => window.gameObject.SetActive(true),
        window => {
          window.Set();
          window.gameObject.SetActive(false);
        },
        window => Destroy(window.gameObject));
    }

    public void ShowMsgBox(string title, string text, string yesText = "예", string noText = "아니요",
                           UnityAction<bool> callback = null) {
      var window = msgBoxPool.Get();
      window.title = title;
      window.Set(text, yesText, noText, callback);
    }

    public void ShowInputBox(string title, string text, UnityAction<string> callback, string defValue = "",
                             string confirm = "확인", string cancel = "취소") {
      var window = inputBoxPool.Get();
      window.title = title;
      window.Set(text, confirm, cancel, defValue, callback);
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