using System;
using UnityEngine;

namespace Dialogue {
  [Serializable]
  public class Speaker {
    public string name;
    
    public Sprite sprite;
    
    public AvatarPosition avatarPosition;
    
    public Color nameColor = Color.white;

    public Speaker(string name, Sprite sprite, AvatarPosition avatarPosition) {
      this.name = name;
      this.sprite = sprite;
      this.avatarPosition = avatarPosition;
    }
  }
}