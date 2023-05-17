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

    private Vector3 npcPos;

    private void Awake() {
      npcCol = GetComponent<BoxCollider2D>();
      npcPos = transform.position;
    }

    private void Start() {
      InvokeRepeating("ShowMessageRandom", 6f, 12f);
      nameTag = (DisplayText)EntityManager.Get(EntityType.DisplayText);
      nameTag.SetText(name);
      nameTag.width = nameTagWidth;
      nameTag.position = new Vector3(npcPos.x,
        npcPos.y + npcCol.bounds.size.y + 0.05f, npcPos.z);
    }

    private void ShowMessageRandom() {
      var msgData = new MessageData(name, messages.Random()) {
        panelWidth = MessageWidth
      };
      messageBox = (MessageBox)EntityManager.Get(EntityType.MessageBox);
      messageBox.ShowMessage(this, msgData);
      messageBox.position = new Vector3(npcPos.x, npcPos.y + npcCol.bounds.size.y + 0.6f, npcPos.z);
    }
  }
}