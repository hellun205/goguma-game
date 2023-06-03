using Animation;
using Manager;
using UnityEngine;
using Utils;

namespace Window
{
  [RequireComponent(typeof(CanvasGroup))]
  public abstract class AnimatedWindow<T> : BaseWindow where T : AnimatedWindow<T>
  {
    private CanvasGroup canvasGroup;
    private SmoothVector3 animSize;
    private StraightFloat animFade;

    private Vector2 defaultSize;
    private readonly Vector3 zero = new Vector3(0.5f, 0.5f, 1f);
    private bool isFadeIn = true;

    protected override void Awake()
    {
      base.Awake();

      canvasGroup = GetComponent<CanvasGroup>();
      defaultSize = transform.localScale.Setter(z: 1f);
      animSize = new SmoothVector3(this, defaultSize, value => transform.localScale = value);
      animFade = new StraightFloat(this, 0f, value => canvasGroup.alpha = value);

      Managers.Window.onGet += WindowOnGet;
      animFade.onEnded += AnimFadeOnEnded;
    }

    private void WindowOnGet(BaseWindow sender)
    {
      if (sender != this) return;
      interactable = true;
      isFadeIn = true;
      animSize.Start(zero, defaultSize, 8f);
      animFade.Start(0f, 1f, 6f);
    }

    protected sealed override void OnCloseButtonClick()
    {
      if (!interactable) return;
      isFadeIn = false;
      interactable = false;
      animSize.Start(transform.localScale, zero, 8f);
      animFade.Start(1f, 0f, 6f);
    }

    private void AnimFadeOnEnded(StraightFloat sender)
    {
      if (!isFadeIn) Managers.Window.Release((T) this);
    }
  }
}
