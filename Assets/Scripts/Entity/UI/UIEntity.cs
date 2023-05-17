using System;
using UnityEngine;
using Utils;

namespace Entity.UI {
  public abstract class UIEntity : Entity {
    public RectTransform rectTransform;
    private Vector3 _position;
    private Vector2 _size;

    public override Vector3 position {
      get => _position;
      set {
        _position = value; 
        RefreshPosition();
      }
    }

    public override Vector2 size {
      get => _size;
      set {
        _size = value;
        rectTransform.sizeDelta = value.WorldToScreenPoint();
      }
    }

    protected virtual void Awake() {
      rectTransform = GetComponent<RectTransform>();

      _position = rectTransform.position.ScreenToWorldPoint();
      _size = rectTransform.sizeDelta.ScreenToWorldPoint();
    }

    protected virtual void Update() {
      RefreshPosition();
    }

    protected void RefreshPosition() {
      rectTransform.position = position.WorldToScreenPoint();
    }
  }
}