using System.Collections;
using System.Linq;
using Dialogue;
using Entity.Player;
using Entity.UI;
using Manager;
using UnityEngine;
using Utils;

namespace Entity.Npc
{
  public class ENpc : Entity
  {
    public Npc npcData;

    [InspectorName("Position")]
    public Transform MessageBoxPosition;

    private UEMsgBox messageBox;

    private Animator anim;

    private Coroutine messageCoroutine;

    public new Vector2 position
    {
      get => base.position;
      set
      {
        base.position = value;
        RefreshPosition();
      }
    }

    private void Awake()
    {
      anim = GetComponent<Animator>();
    }

    private IEnumerator ShowMessage(float delay)
    {
      while (true)
      {
        yield return new WaitForSeconds(delay);

        var msgData = new MessageData(npcData.messages.Random());
        messageBox = Managers.Entity.GetEntity<UEMsgBox>(MessageBoxPosition.position,
          x => x.Init(msgData, () => SetTalking(false)));
        SetTalking(true);
      }
    }

    private void StopMessage()
    {
      if (messageCoroutine is not null)
        StopCoroutine(messageCoroutine);

      SetTalking(false);
    }

    private void RefreshPosition()
    {
      if (messageBox.position != (Vector2) MessageBoxPosition.position)
        messageBox.position = MessageBoxPosition.position;
    }

    private void SetTalking(bool value) => anim.SetBool("isTalking", value);

    public void Meet()
    {
      DialogueController.Instance.ShowDialogues(
        npcData.meetDialogue.Select(dialogue =>
          new DialogueData(
            (
              dialogue.speaker == Speaker.Npc
                ? npcData.speakerData
                : PlayerController.Instance.speakerData
            ),
            dialogue.text
          )).ToArray(), btn =>
        {
          if (npcData.type == NpcType.Shop)
          {
            DialogueController.Instance.Ask(new DialogueData(npcData.speakerData, "상점을 여시겠습니까?"), openShop =>
            {
              if (openShop)
                Debug.Log("open Shop!");
            });
          }
        }
      );
    }

    public override void OnGet()
    {
      base.OnGet();
      messageCoroutine = StartCoroutine(ShowMessage(12f));
    }

    public override void OnRelease()
    {
      base.OnRelease();
      StopMessage();
    }

    public void Init(Npc npc)
    {
      this.npcData = npc;
      anim.runtimeAnimatorController = npc.animCtrler;
      entityName = npc._name;
    }

    public void Init(string uniqueName) => Init(Managers.Npc.GetObject(uniqueName));
  }
}
