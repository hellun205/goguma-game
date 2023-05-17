using System;
using Camera;
using Dialogue;
using Entity.Npc;
using Entity.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Entity {
  public class MessageBox : UIEntity {
    public override EntityType type => EntityType.MessageBox;

    [SerializeField]
    private TextMeshProUGUI text;

    public void ShowMessage(NpcController npc, MessageData messageData) {
      text.text = messageData.text;
      rectTransform.sizeDelta = new Vector2(messageData.panelWidth, 
        MessageData.emptyHeight + MessageData.fontHeight * messageData.line);
      RefreshPosition();
      Invoke("Close", messageData.exitTime);
    }

    private void Close() => Release();
    
  }
}