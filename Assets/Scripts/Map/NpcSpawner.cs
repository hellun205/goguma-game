using System;
using UnityEngine;

namespace Map
{
  public class NpcSpawner : MonoBehaviour
  {
    public string npcName;
    
    public void Start()
    {
      Entity.Entity.SummonNpc(transform.position, npcName);
    }
  }
}
