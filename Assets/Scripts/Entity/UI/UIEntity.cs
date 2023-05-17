using System;
using UnityEngine;
using Utils;

namespace Entity.UI {
  public abstract class UIEntity : Entity {
    public RectTransform rectTransform;
    private Vector3 _position;

    public override Vector3 position {
      get => _position;
      set {
        _position = value; 
        RefreshPosition();
      }
    }

    protected virtual void Awake() {
      rectTransform = GetComponent<RectTransform>();
      rectTransform.SetParent(FindObjectOfType<Canvas>().transform);
    }

    protected virtual void Update() {
      RefreshPosition();
    }

    protected void RefreshPosition() {
      rectTransform.position = UnityEngine.Camera.main.WorldToScreenPoint(position);
    }
  }
}