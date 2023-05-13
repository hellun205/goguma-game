using System;
using UnityEngine;

namespace Player {
  public class PlayerController : MonoBehaviour {
    public static PlayerController Instance { get; private set; }

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy() {
      if (Instance == this) Instance = null;
    }
  }
}