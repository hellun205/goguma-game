using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Npc {
  public class NpcManager : MonoBehaviour {
    public static NpcManager Instance { get; protected set; }

    public List<Npc> npcs = new List<Npc>();

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
    }

    public Npc GetNpc(Npcs who) => npcs.Where(npc => npc.who == who).Single();
  }
}