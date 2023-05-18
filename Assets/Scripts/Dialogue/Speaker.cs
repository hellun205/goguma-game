using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue {
  /// <summary>
  /// 화자의 대한 데이터 입니다.
  /// </summary>
  [Serializable]
  public class Speaker {
    /// <summary>
    /// 화자의 이름을 설정합니다.
    /// </summary>
    public string name;
    
    /// <summary>
    /// 화자의 아바타 스프라이트를 설정합니다.
    /// </summary>
    public Sprite avatarSprite;
    
    /// <summary>
    /// 아바타 위치를 설정합니다.
    /// </summary>
    public AvatarPosition avatarPosition;
    
    /// <summary>
    /// 대화 창에서 표시 할 화자의 이름 색을 설정합니다.
    /// </summary>
    public Color nameColor = Color.white;

    /// <summary>
    /// 화자의 대한 데이터를 생성합니다.
    /// </summary>
    /// <param name="name">화자의 이름</param>
    /// <param name="avatarSprite">화자의 아바타</param>
    /// <param name="avatarPosition">아바타 위치</param>
    public Speaker(string name, Sprite avatarSprite, AvatarPosition avatarPosition) {
      this.name = name;
      this.avatarSprite = avatarSprite;
      this.avatarPosition = avatarPosition;
    }
  }
}