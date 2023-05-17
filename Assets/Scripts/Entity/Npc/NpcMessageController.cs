using System;
using Camera;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Entity.Npc {
  public class NpcMessageController : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI text;

    private IObjectPool<NpcMessageController> pool;

    private Image img;

    private NpcController npc;
    private BoxCollider2D npcCol;
    private Vector3 npcPos;

    public void SetPool(IObjectPool<NpcMessageController> pool) => this.pool = pool;

    private void OnBecameInvisible() => pool.Release(this);

    private void Awake() {
      img = GetComponent<Image>();
    }

    private void Update() {
      RefreshPosition();
    }

    public void ShowMessage(NpcController npc, MessageData messageData) {
      this.npc = npc;
      npcCol = npc.GetComponent<BoxCollider2D>();
      npcPos = npc.transform.position;
      
      text.text = $"<b>{messageData.name}</b>\n{messageData.text}";
      img.rectTransform.sizeDelta = new Vector2(messageData.panelWidth,
        MessageData.emptyHeight + MessageData.fontHeight * messageData.line);
      RefreshPosition();
      Invoke("Close", messageData.exitTime);
    }

    private void RefreshPosition() {
      img.rectTransform.position =
        UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(npcPos.x,
          npcPos.y + npcCol.bounds.size.y + 0.3f, npcPos.z));
    }

    private void Close() => pool.Release(this);
  }
}