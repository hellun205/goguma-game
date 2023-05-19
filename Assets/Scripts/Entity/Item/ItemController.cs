using System;
using Entity.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity.Item {
  public class ItemController : Entity {
    public override EntityType type => EntityType.Item;

    public Item itemData;

    [SerializeField]
    private SpriteRenderer sr;

    private Transform srTranform;
    private NameTag nTag;
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

    private void Awake() {
      nTag = GetComponent<NameTag>();
      srTranform = sr.transform;
    }

    private void Update() {
      var pos = srTranform.localPosition;
      
      // transform.Rotate(Vector3.up * (Time.fixedDeltaTime * 10f));
      if (direction == 1 && pos.y >= maxY - limitY) {
        direction = -1;
      } else if (direction == -1 && pos.y <= 0 + limitY) {
        direction = 1;
      }

      var y = Mathf.Lerp(pos.y, direction == 1 ? maxY : 0f, Time.deltaTime * speed);
      srTranform.localPosition = new Vector3(pos.x, y, pos.z);
    }

    public void SetItem(Item data) {
      itemData = data;

      nTag.text = data._name;
      sr.sprite = data.sprite;
    }

    private void Start() {
      SetItem(itemData);
    }
  }
}