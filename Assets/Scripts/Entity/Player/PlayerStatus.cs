using System;
using UnityEngine.Serialization;
using Utils;
using String = Utils.String;

namespace Entity.Player
{
  [Serializable]
  public struct PlayerStatus
  {
    public float maxHp;

    public float hp;

    public float moveSpeed;

    public byte level;

    public ushort exp;

    [FormerlySerializedAs("jumpSpeed")]
    public float jumpPower;

    public static PlayerStatus operator +(PlayerStatus a, PlayerStatus b) => new PlayerStatus()
    {
      maxHp = a.maxHp + b.maxHp,
      hp = a.hp + b.hp,
      moveSpeed = a.moveSpeed + b.moveSpeed,
      jumpPower = a.jumpPower + b.jumpPower,
    };

    public static PlayerStatus operator -(PlayerStatus a, PlayerStatus b) => new PlayerStatus()
    {
      maxHp = a.maxHp - b.maxHp,
      hp = a.hp - b.hp,
      moveSpeed = a.moveSpeed - b.moveSpeed,
      jumpPower = a.jumpPower - b.jumpPower,
    };

    public string GetInfo() =>
      String.GetValueTag("최대 체력", maxHp) +
      String.GetValueTag("체력", hp) +
      String.GetValueTag("이동 속도", moveSpeed) +
      String.GetValueTag("점프력", jumpPower);
  }
}
