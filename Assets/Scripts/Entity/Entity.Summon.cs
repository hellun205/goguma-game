using System;
using Entity.Enemy;
using Entity.Item;
using Entity.Npc;
using Entity.UI;
using UnityEngine;

namespace Entity {
  public partial class Entity {
    public static ItemController SummonItem(Vector2 position, Item.Item item, byte count) {
      var entity = EntityManager.Get(EntityType.Item, position) as ItemController;
      entity.SetItem(item, count);
      return entity;
    }

    public static ItemController SummonItem(Vector2 position, string itemName, byte count)
      => SummonItem(position, ItemManager.Instance.GetWithCode(itemName), count);

    public static NpcController SummonNpc(Vector2 position, Npc.Npc npc) {
      var entity = EntityManager.Get(EntityType.Npc, position) as NpcController;
      entity.SetNpc(npc);
      return entity;
    }

    public static NpcController SummonNpc(Vector2 position, string npcName) =>
      SummonNpc(position, NpcManager.Instance.GetWithCode(npcName));

    public static DisplayText SummonDisplayText(Vector2 position, string text) {
      var entity = EntityManager.Get(EntityType.DisplayText, position) as DisplayText;
      entity.text = text;
      return entity;
    }

    public static MessageBox SummonMsgBox(Vector2 position, MessageData message, Action callback) {
      var entity = EntityManager.Get(EntityType.MessageBox, position) as MessageBox;
      entity.ShowMessage(message, callback);
      return entity;
    }

    public static HealthBar SummonHpBar(Vector2 position, float value, float maxValue) {
      var entity = EntityManager.Get(EntityType.HpBar, position) as HealthBar;
      entity.value = value;
      entity.maxValue = maxValue;
      return entity;
    }

    public static EnemyController SummonEnemy(Vector2 position) {
      var entity = EntityManager.Get(EntityType.Enemy, position) as EnemyController;

      return entity;
    }
  }
}