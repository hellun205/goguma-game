using System;
using UnityEngine;

namespace MainMenu
{
  public class FadeEffecter : MonoBehaviour
  {
    public static FadeEffecter Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }
    
    
  }
}
