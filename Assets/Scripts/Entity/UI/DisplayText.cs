using System;
using Entity.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Entity.UI {
  public class DisplayText : UIEntity {
    public override EntityType type => EntityType.DisplayText;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Image img;

    private byte _line;
    
    public float width {
      get => rectTransform.sizeDelta.x;
      set => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
      }

    public byte line {
      get => _line;
      set {
        _line = value;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _line * 16f + 6f);
      }
    }

    protected override void Awake() {
      base.Awake();

      width = 300f;
      line = 1;
    }

    public void SetText(string text) {
      this.text.text = text;
    }


    // private void OnBecameInvisible() => pool.Release(this);
  }
}