using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObject;
using UnityEngine;

namespace Entity.Npc
{
  public class NpcManager : ScriptableObjectManager<Npc>
  {
    public static NpcManager GetInstance() => (NpcManager)Instance;

  }
}
