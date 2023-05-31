using UnityEngine;
using Utils;

namespace Entity.UI
{
  /// <summary>
  /// UI Entity component
  /// </summary>
  public abstract class UIEntity : Entity
  {
    protected RectTransform rectTransform { get; private set; }

    private static UnityEngine.Camera cam;
    
    private static RectTransform container;

    private Vector3 _position;

    public override Vector2 position
    {
      get => _position;
      set
      {
        _position = value;
        rectTransform.anchoredPosition = container.WorldToScreenSpace(_position);
      }
    }

    protected virtual void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
      cam ??= UnityEngine.Camera.main;
      container ??= GameObject.Find("UIEntityContainer").GetComponent<RectTransform>();
    }
  }
}
