using System;
using System.Linq;
using Dialogue;
using Entity.Player;
using Entity.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Entity.Npc {
  public class NpcController : Entity {
    public override EntityType type => EntityType.Npc;

    public Npc npcData;

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
      StartMessage();
      RefreshPosition();
    }

    private void StartMessage() => InvokeRepeating("ShowMessageRandom", 6f, 12f);

    private void StopMessage() {
      CancelInvoke("ShowMessageRandom");
      SetTalking(false);
    }

    private void ShowMessageRandom() {
      var msgData = new MessageData(entityName, npcData.messages.Random()) {
        panelWidth = MessageWidth
      };
      messageBox = (MessageBox) EntityManager.Get(EntityType.MessageBox);
      SetTalking(true);
      messageBox.ShowMessage(this, msgData, () => SetTalking(false));
      RefreshPosition();
    }

    private void RefreshPosition() {
      if (messageBox != null) messageBox.position = MessageBoxPosition.position;
    }

    private void SetTalking(bool value) => anim.SetBool("isTalking", value);

    public void Meet() {
      DialogueController.Instance.ShowDialogues(npcData.meetDialogue.Select(dialogue =>
        new DialogueData(
          (dialogue.speaker == Speaker.Npc ? npcData.speakerData : PlayerController.Instance.speakerData),
          dialogue.text)).ToArray(), btn => {
        if (npcData.type == NpcType.Shop) {
          DialogueController.Instance.Ask(new DialogueData(npcData.speakerData, "상점을 여시겠습니까?"), openShop => {
            if (openShop) {
              Debug.Log("open Shop!");
            }
          });
        }
      });
    }
  }
}