using System;
using UnityEngine;

namespace Entity.Item
{
  public class ItemController : Entity
  {
    public static EntityType Type => EntityType.Item; 
    public override EntityType type => Type;

    public (Item item, byte count) data;

    [SerializeField]
    private SpriteRenderer[] sprRenderers;

    [SerializeField]
    private byte[] countSprite;

    private Rigidbody2D rb;

    [SerializeField]
    [Range(1f, 10f)]
    private float pickupSpeed = 1f;

    [HideInInspector]
    public bool isPickingUp;

    [HideInInspector]
    public bool isThrowing;

    private Transform target;

    private Action<(Item item, byte count)> callback;

    protected virtual void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
      if (isPickingUp)
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * pickupSpeed);
    }

    public void Init(Item item, byte count = 1)
    {
      this.data = (item, count);
      if (item == null)
      {
        entityName = string.Empty;
        for (var i = 0; i < sprRenderers.Length; i++)
        {
          sprRenderers[i].sprite = ItemManager.GetInstance().noneSprite;
          sprRenderers[i].enabled = false;
        }

        return;
      }

      entityName = $"{item._name}{(count == 1 ? "" : $"x{count}")}";
      for (var i = 0; i < sprRenderers.Length; i++)
      {
        sprRenderers[i].sprite = item.sprite;
        sprRenderers[i].enabled = countSprite[i] <= data.count;
      }
    }

    public void Init(string uniqueName, byte count = 1) =>
      Init(ItemManager.Instance.GetWithCode(uniqueName), count);


    public void PickUp(Transform target, Action<(Item item, byte count)> callback)
    {
      this.callback = callback;
      this.target = target;
      isPickingUp = true;
    }

    public override void Release()
    {
      Init(item: null);
      isPickingUp = false;
      base.Release();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
      if (isPickingUp)
      {
        if (col.transform.name == target.name)
        {
          callback.Invoke(data);
          Release();
        }
      }
      else if (col.CompareTag("Item"))
      {
        var item = col.GetComponent<ItemController>();
        if (item.data.item == this.data.item && item.data.count != byte.MaxValue)
        {
          var plus = item.data.count + this.data.count;
          if (plus <= byte.MaxValue)
          {
            item.Init(item.data.item, (byte)(item.data.count + this.data.count));
            this.Release();
          }
          else
          {
            var left = plus - byte.MaxValue;
            item.Init(item.data.item, byte.MaxValue);
            this.Init(this.data.item, (byte)left);
          }
        }
      }
    }

    public void Throw(Vector2 direction, float power)
    {
      rb.velocity = direction.normalized * power;
      isThrowing = true;
      Invoke(nameof(EndThrowing), 2f);
    }

    private void EndThrowing() => isThrowing = false;
  }
}
