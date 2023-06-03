using Animation.Combined;
using Animation.Preset;
using Manager;
using UnityEngine;

namespace Window
{
  [RequireComponent(typeof(CanvasGroup))]
  public abstract class AnimatedWindow<T> : BaseWindow where T : AnimatedWindow<T>
  {
    private PanelVisibler anim;

    protected override void Awake()
    {
      base.Awake();
      anim = new(this);

      Managers.Window.onGet += WindowOnGet;
      anim.animation.onHid += AnimOnHid;
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
