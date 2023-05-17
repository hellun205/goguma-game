using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Entity.Npc {
  public class NpcController : Entity {
    public override EntityType type => EntityType.Npc;

    [Header("Messages")]
    public string[] messages;

    public float MessageWidth = 150f;

    [InspectorName("Position")]
    public Transform MessageBoxPosition;

    private MessageBox messageBox;

    private BoxCollider2D npcCol;

    private Animator anim;

    public new Vector3 position {
      get => base.position;
      set {
        base.position = value;
        RefreshPosition();
      }
    }

    private void Awake() {
      npcCol = GetComponent<BoxCollider2D>();
      anim = GetComponent<Animator>();
    }

    private void Start() {
      InvokeRepeating("ShowMessageRandom", 6f, 12f);
      RefreshPosition();
    }

    private void ShowMessageRandom() {
      var msgData = new MessageData(entityName, messages.Random()) {
        panelWidth = MessageWidth
      };
      messageBox = (MessageBox)EntityManager.Get(EntityType.MessageBox);
      SetTalking(true);
      messageBox.ShowMessage(this, msgData, () => SetTalking(false));
      RefreshPosition();
    }

    private void RefreshPosition() {
      if (messageBox != null) messageBox.position = MessageBoxPosition.position;
    }

    private void SetTalking(bool value) => anim.SetBool("isTalking", value);
  }
}