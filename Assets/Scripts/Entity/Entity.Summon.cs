using System;
using Entity.Enemy;
using Entity.Item;
using Entity.Npc;
using Entity.UI;
using UnityEngine;

namespace Entity {
  public partial class Entity {
    public static ItemController SummonItem(Vector2 position, Item.Item item, byte count) {
      var entity = EntityManager.Get(EntityType.Item) as ItemController;
      entity.SetItem(item, count, position);
      return entity;
    }

    public static NpcController SummonNpc(Vector2 position, Npc.Npc npc) {
      var entity = EntityManager.Get(EntityType.Npc) as NpcController;
      entity.Initialize(npc, position);
      return entity;
    }

    public static DisplayText SummonDisplayText(Vector2 position) {
      var entity = EntityManager.Get(EntityType.DisplayText) as DisplayText;
      entity.position = position;
      return entity;
    }

    public static MessageBox SummonMsgBox(Vector2 position, MessageData message, Action callback) {
      var entity = EntityManager.Get(EntityType.MessageBox) as MessageBox;
      entity.position = position;
      entity.ShowMessage(message, callback);
      return entity;
    }

    public static HealthBar SummonHpBar(Vector2 position, float value, float maxValue) {
      var entity = EntityManager.Get(EntityType.HpBar) as HealthBar;
      entity.position = position;
      entity.value = value;
      entity.maxValue = maxValue;
      return entity;
    }

    public static EnemyController SummonEnemy(Vector2 position) {
      var entity = EntityManager.Get(EntityType.Enemy) as EnemyController;
      entity.position = position;
      
      return entity;
    }
  }
}