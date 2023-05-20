using System;

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
  }
}