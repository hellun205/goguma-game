using System;
using Dialogue;
using UnityEngine;

namespace Entity.Npc {
  [Serializable]
  public class Npc {
    public string name;

    public NpcType type;

    public Npcs who; 
    
    public string[] messages;

    public NpcDialogue[] meetDialogue;

    public ShopData shopData;

    public Sprite sprite;

    public Dialogue.Speaker speakerData => new Dialogue.Speaker(name, sprite, AvatarPosition.Right);
  }
}