using System;
using Utils;

namespace Entity.Player {
  [Serializable]
  public struct PlayerStatus {
    public float maxHp;
    
    public float hp;

    public float moveSpeed;

    public float jumpSpeed;

    public static PlayerStatus operator +(PlayerStatus a, PlayerStatus b) => new PlayerStatus() {
      maxHp = a.maxHp +b.maxHp,
      hp = a.hp +b.hp,
      moveSpeed = a.moveSpeed +b.moveSpeed,
      jumpSpeed = a.jumpSpeed +b.jumpSpeed,
    };

    public static PlayerStatus operator -(PlayerStatus a, PlayerStatus b) => new PlayerStatus() {
      maxHp = a.maxHp - b.maxHp,
      hp = a.hp - b.hp,
      moveSpeed = a.moveSpeed - b.moveSpeed,
      jumpSpeed = a.jumpSpeed - b.jumpSpeed,
    };
    
    public string GetInfo() =>
      StringUtils.GetValueTag("최대 체력", maxHp) +
      StringUtils.GetValueTag("체력", hp) +
      StringUtils.GetValueTag("이동 속도", moveSpeed) +
      StringUtils.GetValueTag("점프력", jumpSpeed);
  }
}