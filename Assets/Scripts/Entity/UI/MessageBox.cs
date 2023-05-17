using System;
using Camera;
using Dialogue;
using Entity.Npc;
using Entity.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Entity.UI {
  public class MessageBox : UIEntity {
    public override EntityType type => EntityType.MessageBox;

    [SerializeField]
    private TextMeshProUGUI text;

    [CanBeNull]
    private Action callBack;

    public void ShowMessage(NpcController npc, MessageData messageData, [CanBeNull] Action endCallback = null) {
      text.text = messageData.text;
      rectTransform.sizeDelta = new Vector2(messageData.panelWidth, 
        MessageData.emptyHeight + MessageData.fontHeight * messageData.line);
      callBack = endCallback;
      RefreshPosition();
      Invoke("Close", messageData.exitTime);
    }

    private void Close()  {
      callBack?.Invoke();
      Release();
    }
    
  }
}