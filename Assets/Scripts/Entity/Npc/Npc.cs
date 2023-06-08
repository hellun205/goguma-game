using System;
using System.Collections.Generic;
using Dialogue;
using Quest;
using UnityEditor.Animations;
using UnityEngine;

namespace Entity.Npc
{
  [Serializable]
  public abstract class Npc : ScriptableObject
  {
    public abstract NpcType type { get; }

    [Header("Npc")]
    public string _name;

    public Sprite avatar;

    public AnimatorController animCtrler;

    [Header("Dialogue")]
    public string[] messages;

    public NpcDialogue[] meetDialogue;

    public Dialogue.Speaker speakerData => new Dialogue.Speaker(_name, avatar, AvatarPosition.Right);

    [Header("Quest")]
    public NpcQuestData[] quest;
  }
}
