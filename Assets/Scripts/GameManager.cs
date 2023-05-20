using UnityEngine;

namespace DefaultNamespace {
  public class GameManager:MonoBehaviour {
    public static GameManager Instance { get; private set; }

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(this);
      DontDestroyOnLoad(gameObject);
    }
  }
}