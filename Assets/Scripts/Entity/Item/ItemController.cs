using System;
using Entity.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity.Item {
  public class ItemController : Entity {
    public override EntityType type => EntityType.Item;

    public (Item item, byte count) data;

    [SerializeField]
    private SpriteRenderer sr;

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

    private Transform target;

    private Action<(Item item, byte count)> callback;

    private void Awake() {
      nTag = GetComponent<NameTag>();
      rb = GetComponent<Rigidbody2D>();
      imgTrans = sr.transform;
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
        sr.sprite = ItemManager.GetInstance().noneSprite;
        return;
      }

      entityName = $"{item._name}{(count == 1 ? "" : $"x{count}")}";
      sr.sprite = item.sprite;
      if (position.HasValue)
        transform.position = position.Value;
    }

    public void SetItem(string uniqueName,byte count =1, Vector3? position = null) =>
      SetItem(ItemManager.Instance.GetWithCode(uniqueName),count, position);


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
      } else {
        
      }
    }
  }
}