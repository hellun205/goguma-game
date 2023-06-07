using System.Collections;
using System.Collections.Generic;
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

    public Transform MessageBoxPosition;

    private UEMsgBox messageBox;

    [SerializeField]
    private GameObject exclamation;

    [SerializeField]
    private GameObject question;

    [SerializeField]
    private GameObject dots;

    private Animator anim;

    private Coroutine messageCoroutine;

    public List<int> receivableQuests;
    public List<int> completableQuests;

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
        SetQuestBoxPos(1.25f);
        var msgData = new MessageData(npcData.messages.Random());
        messageBox = Managers.Entity.Get<UEMsgBox>(MessageBoxPosition.position,
          x => x.Init(msgData, () =>
          {
            SetTalking(false);
            SetQuestBoxPos(0.75f);
          }));
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
      if (completableQuests.Any())
      {
        var quest = Managers.Quest.GetQuestByID(completableQuests[0]);
        Managers.Dialogue.ShowDialogues(GetDialogueData(quest.completeDialogue), _ =>
        {
          if (quest.rewards.Any(reward => !reward.Compensate()))
            Managers.Dialogue.ShowDialogue(GetDialogueData(quest.cantCompensateDialogue));
          else
          {
            Managers.Player.questData.endedQuest.Add(quest.index);
            Managers.Player.questData.RemoveQuest(quest.index);
            RefreshQuest();
          }
        });
        return;
      }
      
      if (receivableQuests.Any())
      {
        var quest = Managers.Quest.GetQuestByID(receivableQuests[0]);
        Managers.Dialogue.ShowDialogues(GetDialogueData(quest.dialogue), _ =>
        {
          Managers.Dialogue.Ask(GetDialogueData(quest.askDialogue), accept =>
          {
            if (accept)
            {
              Managers.Dialogue.ShowDialogues(GetDialogueData(quest.acceptDialogue));
              Managers.Player.questData.quests.Add(quest.GetQuestInfo());
              RefreshQuest();
            }
            else
              Managers.Dialogue.ShowDialogues(GetDialogueData(quest.refuseDialogue));
          });
        });
        return;
      }

      DialogueController.Instance.ShowDialogues(GetDialogueData(npcData.meetDialogue), _ =>
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
      SetQuestBoxPos(0.75f);

      messageCoroutine = StartCoroutine(ShowMessage(12f));
    }

    public void SetQuestBoxPos(float value)
    {
      var pos = position.Plus(y: col.bounds.extents.y + value);
      exclamation.transform.position = pos;
      question.transform.position = pos;
      dots.transform.position = pos;
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
      RefreshQuest();
    }

    public void Init(string uniqueName) => Init(Managers.Npc.GetObject(uniqueName));

    public void RefreshQuest()
    {
      var player = Managers.Player;
      completableQuests.Clear();
      receivableQuests.Clear();
      question.SetActive(false);
      dots.SetActive(false);
      exclamation.SetActive(false);
      
      var receivable = npcData.quest.Where(quest =>
        player.questData.quests.All(info => info.questIndex == quest.questID) &&
        !player.questData.endedQuest.Contains(quest.questID) &&
        quest.requireLevel <= player.status.level &&
        (
          quest.requireQuests.Length == 0 ||
          quest.requireQuests.All(require => player.questData.endedQuest.Contains(require))
        )
      ).Select(x => x.questID).ToArray();

      var completable = player.questData.quests.Where(quest =>
        npcData.quest.Any(data => data.questID == quest.questIndex) &&
        quest.isCompleted
      ).Select(x => x.questIndex).ToArray();

      if (receivable.Any())
        receivableQuests.AddRange(receivable);
      if (completable.Any())
        completableQuests.AddRange(completable);
      var receivedQuest =
        player.questData.quests.Any(info => npcData.quest.Any(data => data.questID == info.questIndex));

      if (completableQuests.Any())
        question.SetActive(true);
      else if (receivedQuest)
        dots.SetActive(true);
      else if (receivableQuests.Any())
        exclamation.SetActive(true);
    }

    private DialogueData[] GetDialogueData(IEnumerable<NpcDialogue> npcDialogues)
    {
      return npcDialogues.Select(GetDialogueData).ToArray();
    }

    private DialogueData GetDialogueData(NpcDialogue npcDialogue)
    {
      return new DialogueData(
        npcDialogue.speaker == Speaker.Npc ? npcData.speakerData : PlayerController.Instance.speakerData,
        npcDialogue.text);
    }

    public static void RefreshQuestAll()
    {
      var npcs = FindObjectsOfType<ENpc>();
      foreach (var npc in npcs.Where(npc => npc.gameObject.activeSelf))
      {
        npc.RefreshQuest();
      }
    }
  }
}
