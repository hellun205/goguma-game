using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Entity.UI {
  /// <summary>
  /// 메시지 박스 엔티티 컴포넌트
  /// </summary>
  public class MessageBox : UIEntity {
    public override EntityType type => EntityType.MessageBox;

    /// <summary>
    /// 메시지 박스의 내용을 표시할 TextMeshProUGUI 컴포넌트를 가져옵니다.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text;

    /// <summary>
    /// 메시지 박스가 표시된 후 실행되는 콜백함수 입니다.
    /// </summary>
    [CanBeNull]
    private Action callBack;

    /// <summary>
    /// 메시지 박스를 설정합니다.
    /// </summary>
    /// <param name="messageData">메시지 박스를 설정 할 데이터</param>
    /// <param name="endCallback">메시지 박스가 종료된 후 실행 할 콜백함수</param>
    public void ShowMessage(MessageData messageData, [CanBeNull] Action endCallback = null) {
      text.text = messageData.text;
      callBack = endCallback;
      RefreshPosition();
      Invoke("Close", messageData.exitTime);
    }

    /// <summary>
    /// 메시지 박스를 종료합니다.
    /// </summary>
    public void Close()  {
      callBack?.Invoke();
      Release();
    }
    
  }
}