using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Entity.UI
{
  public class UEMsgBox : UIEntity
  {
    [SerializeField]
    private TextMeshProUGUI text;
    
    [CanBeNull]
    private Action callBack;
    
    public void Init(MessageData messageData, [CanBeNull] Action endCallback = null)
    {
      text.text = messageData.text;
      callBack = endCallback;
      Invoke(nameof(Close), messageData.exitTime);
    }
    
    public void Close()
    {
      callBack?.Invoke();
      Release();
    }
  }
}
