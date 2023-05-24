using System.Linq;
using Dialogue;
using Entity.Player;
using Entity.UI;
using UnityEngine;
using Utils;

namespace Entity.Npc {
  /// <summary>
  /// NPC 컴포넌트
  /// </summary>
  public class NpcController : Entity {
    public override EntityType type => EntityType.Npc;

    public Npc npcData;

    public float MessageWidth = 150f;

    [InspectorName("Position")]
    public Transform MessageBoxPosition;

    private MessageBox messageBox;

    private Animator anim;

    public new Vector3 position {
      get => base.position;
      set {
        base.position = value;
        RefreshPosition();
      }
    }

    private void Awake() {
      anim = GetComponent<Animator>();
    }

    private void Start() {
      StartMessage();
    }

    private void StartMessage() => InvokeRepeating(nameof(ShowMessageRandom), 6f, 12f);

    private void StopMessage() {
      CancelInvoke(nameof(ShowMessageRandom));
      SetTalking(false);
    }

    private void ShowMessageRandom() {
      var msgData = new MessageData(npcData.messages.Random());
      messageBox = (MessageBox) EntityManager.Get(EntityType.MessageBox);
      SetTalking(true);
      messageBox.ShowMessage(msgData, () => SetTalking(false));
      RefreshPosition();
    }

    private void RefreshPosition() {
      if (messageBox.position != MessageBoxPosition.position)
        messageBox.position = MessageBoxPosition.position;
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

    public override void OnGet() {
      base.OnGet();
      StartMessage();
    }

    public override void OnRelease() {
      base.OnRelease();
      StopMessage();
    }

    public void Initialize(Npc npc, Vector2? position = null) {
      this.npcData = npc;
      anim.runtimeAnimatorController = npc.animCtrler;
      if (position.HasValue)
        transform.position = position.Value;
      entityName = npc._name;
    }

    public void Initialize(string uniqueName, Vector2? position = null) =>
      Initialize(NpcManager.Instance.GetWithCode(uniqueName), position);
  }
}