using System;
using System.Collections.Generic;
using Dialogue;
using Entity.Npc;
using Manager;
using Quest.User;
using UnityEngine;

namespace Quest
{
  [CreateAssetMenu(fileName = "Quest", menuName = "Quest/Quest", order = 0)]
  public class Quest : ScriptableObject
  {
    public string _name;
    public int index;
    
    [Multiline]
    public string descriptions;

    public List<QuestContent> contents;

    [Header("Dialogue")]
    public NpcDialogue[] dialogue;

    public NpcDialogue askDialogue;

    public NpcDialogue[] acceptDialogue;
    
    public NpcDialogue[] refuseDialogue;

    public QuestInfo GetQuestInfo()
    {
      var info = new QuestInfo
      {
        questIndex = index,
      };
      
      foreach (var content in contents)
      {
        info.requires.Add(content switch
        {
          QKillEnemy killEnemy     => new NeedKillEnemy(killEnemy.enemyName, killEnemy.count),
          QRequireItem requireItem => new NeedItem(requireItem.itemName, requireItem.count),
          _                        => throw new NotImplementedException(),
        });
      }

      return info;
    }
  }
}
