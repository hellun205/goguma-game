using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace Window {
  public class MessageBoxManager : MonoBehaviour {
    public static MessageBoxManager Instance { get; private set; }

    private IObjectPool<MessageBoxWindow> pool;

    [SerializeField]
    private MessageBoxWindow prefab;

    [SerializeField]
    private RectTransform container;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(this);

      pool = new ObjectPool<MessageBoxWindow>(
        () => {
          var window = Instantiate(prefab, container);
          window.pool = pool;
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
      var window = pool.Get();
      window.title = title;
      window.Set(text, yesText, noText, callback);
    }

    public static void Ask(string title, string text, string yesText = "예", string noText = "아니요",
                            UnityAction<bool> callback = null)
      => Instance.ShowMsgBox(title, text, yesText, noText, callback);

    public static void Show(string title, string text, string comfirmText = "확인", UnityAction callBack = null)
      => Instance.ShowMsgBox(title, text, comfirmText, string.Empty, b => callBack?.Invoke());
  }
}