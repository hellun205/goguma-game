using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Entity.Npc {
  public class NpcController : Entity {
    public override EntityType type => EntityType.Npc;

    [Header("Name Tag")]
    public float nameTagWidth = 50f;

    [Header("Messages")]
    public string[] messages;

    [FormerlySerializedAs("width")]
    public float MessageWidth = 150f;

    private MessageBox messageBox;
    private DisplayText nameTag;

    private BoxCollider2D npcCol;


    public new Vector3 position {
      get => base.position;
      set {
        base.position = value;
        RefreshPosition();
      }
    }

    private void Awake() {
      npcCol = GetComponent<BoxCollider2D>();
    }

    private void Start() {
      InvokeRepeating("ShowMessageRandom", 6f, 12f);
      nameTag = (DisplayText)EntityManager.Get(EntityType.DisplayText);
      nameTag.SetText(name);
      nameTag.width = nameTagWidth;
      RefreshPosition();
    }

    private void ShowMessageRandom() {
      var msgData = new MessageData(name, messages.Random()) {
        panelWidth = MessageWidth
      };
      messageBox = (MessageBox)EntityManager.Get(EntityType.MessageBox);
      messageBox.ShowMessage(this, msgData);
      RefreshPosition();
    }

    private void RefreshPosition() {
      var npcPos = transform.position;
      if (messageBox != null) messageBox.position = new Vector3(npcPos.x, npcPos.y + npcCol.bounds.size.y, npcPos.z);
      if (nameTag != null) nameTag.position = new Vector3(npcPos.x, npcPos.y + npcCol.bounds.size.y, npcPos.z);
    }
  }
}