using System;
using Entity.Npc;
using Manager;
using UnityEngine;

namespace Map
{
  public class NpcSpawner : MonoBehaviour
  {
    public string npcName;

    public void Start()
    {
      Managers.Entity.GetEntity<NpcController>(transform.position, x => x.Init(npcName));
    }
  }
}
