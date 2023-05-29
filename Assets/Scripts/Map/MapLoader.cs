using UnityEngine;

namespace Map
{
  public class MapLoader : MonoBehaviour
  {
    public Transform playerStartPos;

    public const string PlayerStartPosition = "PlayerStartPosition";
    
    private void Awake()
    {
      playerStartPos = GameObject.FindWithTag(PlayerStartPosition).transform;
      
    }
  }
}
