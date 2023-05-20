using System;
using Entity.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.Item {
  public class ItemController : Entity {
    public override EntityType type => EntityType.Item;

    public (Item item, byte count) data;

    [SerializeField]
    private SpriteRenderer[] sprRenderers;

    [SerializeField]
    private byte[] countSprite;

    [SerializeField]
    private Transform imgTrans;

    private NameTag nTag;
    private Rigidbody2D rb;
    private int direction = 1;

    [SerializeField]
    [Range(0.2f, 0.4f)]
    private float maxY = 0.2f;

    [SerializeField]
    [Range(0f, 0.1f)]
    private float limitY = 0.05f;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float speed = 10f;

    [SerializeField]
    [Range(1f, 10f)]
    private float pickupSpeed = 1f;

    [HideInInspector]
    public bool isPickingUp;
    
    [HideInInspector]
    public bool isThrowing;

    private Transform target;

    private Action<(Item item, byte count)> callback;

    private void Awake() {
      nTag = GetComponent<NameTag>();
      rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
      if (!isPickingUp) {
        var pos = imgTrans.localPosition;

        // transform.Rotate(Vector3.up * (Time.fixedDeltaTime * 10f));
        if (direction == 1 && pos.y >= maxY - limitY) {
          direction = -1;
        } else if (direction == -1 && pos.y <= 0 + limitY) {
          direction = 1;
        }

        var y = Mathf.Lerp(pos.y, direction == 1 ? maxY : 0f, Time.deltaTime * speed);
        imgTrans.localPosition = new Vector3(pos.x, y, pos.z);
      } else {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * pickupSpeed);
      }
    }

    public void SetItem(Item item, byte count = 1, Vector3? position = null) {
      this.data = (item, count);
      if (item == null) {
        entityName = string.Empty;
        for (var i = 0; i < sprRenderers.Length; i++) {
          sprRenderers[i].sprite = ItemManager.GetInstance().noneSprite;
          sprRenderers[i].enabled = false;
        }

        return;
      }
      
      entityName = $"{item._name}{(count == 1 ? "" : $"x{count}")}";
      for (var i = 0; i < sprRenderers.Length; i++) {
        sprRenderers[i].sprite = item.sprite;
        sprRenderers[i].enabled = countSprite[i] <= data.count;
      }

      if (position.HasValue)
        transform.position = position.Value;
    }

    public void SetItem(string uniqueName, byte count = 1, Vector3? position = null) =>
      SetItem(ItemManager.Instance.GetWithCode(uniqueName), count, position);


    public void PickUp(Transform target, Action<(Item item, byte count)> callback) {
      this.callback = callback;
      this.target = target;
      isPickingUp = true;
    }

    public override void Release() {
      SetItem(item: null);
      isPickingUp = false;
      base.Release();
    }

    private void OnTriggerStay2D(Collider2D col) {
      if (isPickingUp) {
        if (col.transform.name == target.name) {
          callback.Invoke(data);
          Release();
        }
      } else if (col.CompareTag("Item")) {
        var item = col.GetComponent<ItemController>();
        if (item.data.item == this.data.item && item.data.count != byte.MaxValue) {
          var plus = item.data.count + this.data.count;
          if (plus <= byte.MaxValue) {
            item.SetItem(item.data.item, (byte) (item.data.count + this.data.count));
            this.Release();
          } else {
            var left = plus - byte.MaxValue;
            item.SetItem(item.data.item, byte.MaxValue);
            this.SetItem(this.data.item, (byte)left);
          }
        }
      }
    }

    public void Throw(Vector2 startPosition, Vector2 direction, float power) {
      position = startPosition;
      rb.velocity = direction.normalized * power;
      isThrowing = true;
      Invoke(nameof(EndThrowing), 2f);
    }

    private void EndThrowing() => isThrowing = false;


  }
}