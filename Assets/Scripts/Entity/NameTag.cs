using System;
using Entity.UI;
using UnityEngine;

namespace Entity {
  public class NameTag : MonoBehaviour {
    private string _text;
    private Vector3 tempPosition;
    
    public string text {
      get => _text;
      set {
        _text = value;
        Refresh();
      }
    }

    private DisplayText obj;
    private Entity entity;
    
    [SerializeField]
    private Transform position;

    private void Awake() {
      entity = GetComponent<Entity>();

      _text = entity.entityName;
      tempPosition = entity.position;
    }

    private void Start() {
      obj = (DisplayText) EntityManager.Get(EntityType.DisplayText);
      obj.width = 10f * text.Length + 20f;
      Refresh();
    }

    private void Update() {
      if (tempPosition == entity.position) return;
      tempPosition = entity.position;
      Refresh();
    }

    private void Refresh() {
      obj.SetText(text);
      obj.position = position.position;
    }
  }
}