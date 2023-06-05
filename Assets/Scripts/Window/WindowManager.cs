using System;
using Manager;
using Pool;
using UnityEngine;
using UnityEngine.Events;

namespace Window
{
  public sealed class WindowManager : PoolManager<WindowManager, BaseWindow>
  {
    public bool IsActive => count > 0;

    private byte count;

    protected override void Awake()
    {
      base.Awake();
      parent = GameObject.Find("@WindowContainer").transform;
      onGetAfter += _ => count++;
      onReleaseBefore += _ => count--;
    }

    public MessageBoxWindow Ask(
      string title,
      string text,
      string yesText = "예",
      string noText = "아니요",
      UnityAction<bool> callback = null
    ) =>
      Get<MessageBoxWindow>(msgBox =>
      {
        msgBox.title = title;
        msgBox.Init(text, yesText, noText, callback);
      });

    public MessageBoxWindow ShowMsg(
      string title,
      string text,
      string yesText = "확인",
      UnityAction callback = null
    )
      => Ask(title, text, yesText, string.Empty, res => callback?.Invoke());

    public InputBoxWindow ReadText(
      string title,
      string text,
      UnityAction<string> callback,
      string defValue = "",
      string confirm = "확인",
      string cancel = "취소"
    ) =>
      Get<InputBoxWindow>(inputBox =>
      {
        inputBox.title = title;
        inputBox.Init(text, confirm, cancel, defValue, callback);
      });

    public void ReadInt(
      string title,
      UnityAction<int> callback,
      string text = "",
      int defValue = 0,
      string confirm = "확인",
      string cancel = "취소",
      int minValue = Int32.MinValue,
      int maxValue = Int32.MaxValue
    )
    {
      string @return = "";
      int returnInt;

      void ShowInputBox()
      {
        ReadText(title, text, str =>
        {
          if (Int32.TryParse(str, out returnInt))
            if (returnInt > maxValue || returnInt < minValue)
              ShowMsg(title, $"{minValue}~{maxValue} 사이의 값을 입력하세요.", callback: ShowInputBox);
            else
              callback.Invoke(returnInt);
          else
            ShowMsg(title, "정수를 입력하세요", callback: ShowInputBox);
        }, defValue.ToString(), confirm, cancel);
      }

      ShowInputBox();
    }
  }
}
