using System;
using Dialogue;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.Npc
{
  [Serializable]
  public abstract class Npc : UnityEngine.ScriptableObject
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
  }
}
