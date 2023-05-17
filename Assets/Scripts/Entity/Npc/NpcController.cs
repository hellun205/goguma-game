using System;
using UnityEngine;
using Utils;

namespace Entity.Npc {
  public class NpcController : MonoBehaviour {
    public string name;
    
    [Header("Messages")]
    public string[] messages;

    public float width = 150f; 

    private void Start() {
      InvokeRepeating("ShowMessageRandom", 3f, 5f);
    }

    private void ShowMessageRandom() {
      var msgData = new MessageData(name, messages.Random()) {
        panelWidth = width
      };
      
      NpcMessageManager.Instance.ShowMessage(this, msgData);
    }
  }
}