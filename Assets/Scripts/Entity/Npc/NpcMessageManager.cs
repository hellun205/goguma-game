using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity.Npc {
  public class NpcMessageManager : MonoBehaviour {
    public static NpcMessageManager Instance { get; private set; }

    [SerializeField]
    private NpcMessageController messageController;

    private IObjectPool<NpcMessageController> pool;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);

      pool = new ObjectPool<NpcMessageController>(
        () => {
          var obj = Instantiate(messageController);
          obj.gameObject.transform.parent = gameObject.transform;
          obj.SetPool(pool);
          return obj;
        },
        npc => npc.gameObject.SetActive(true),
        npc => npc.gameObject.SetActive(false),
        npc => Destroy(npc.gameObject),
        maxSize: 5);
    }

    public void ShowMessage(NpcController npc, MessageData messageData) {
      var npcMessage = pool.Get();
      
      npcMessage.ShowMessage(npc, messageData);
      Debug.Log("Show Message");
    }
    
  }
}