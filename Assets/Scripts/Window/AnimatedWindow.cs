using Animation;
using Animation.Combined;
using Manager;
using UnityEngine;
using Utils;

namespace Window
{
  [RequireComponent(typeof(CanvasGroup))]
  public abstract class AnimatedWindow<T> : BaseWindow where T : AnimatedWindow<T>
  {
    private CanvasGroup canvasGroup;

    // private SmoothVector3 animSize;
    // private StraightFloat animFade;
    private SmoothSizeAndFade anim;

    private Vector2 defaultSize;
    private readonly Vector2 zero = new (0.8f, 0.8f);

    protected override void Awake()
    {
      base.Awake();

      canvasGroup = GetComponent<CanvasGroup>();
      defaultSize = transform.localScale;
      // animSize = new(this, new(() => transform.localScale, value => transform.localScale = value));
      // animFade = new(this, new(() => canvasGroup.alpha, value => canvasGroup.alpha = value));
      anim = new
      (
        this,
        new(() => transform.localScale, value => transform.localScale = value),
        new(() => canvasGroup.alpha, value => canvasGroup.alpha = value)
      )
      {
        minSize = zero,
        maxSize = defaultSize,
        fadeAnimSpeed = 6f,
        sizeAnimSpeed = 8f
      };

      Managers.Window.onGet += WindowOnGet;
      anim.onHid += AnimOnHid;
    }

    private void WindowOnGet(BaseWindow sender)
    {
      if (sender != this) return;
      interactable = true;
      anim.Show();
    }

    protected sealed override void OnCloseButtonClick()
    {
      if (!interactable) return;
      interactable = false;
      anim.Hide();
    }
    
    private void AnimOnHid(SmoothSizeAndFade sender)
    {
      Managers.Window.Release((T) this);
    }
  }
}
