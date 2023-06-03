using Animation.Combined;
using UnityEngine;

namespace Animation.Preset
{
  public sealed class PanelVisibler : CombinedAnimationPreset<SmoothSizeAndFade>
  {
    private CanvasGroup canvasGroup { get; }
    private Transform transform { get; }

    public PanelVisibler(MonoBehaviour sender) : base(sender)
    {
      // Set Components
      canvasGroup = sender.GetComponent<CanvasGroup>();
      transform = sender.transform;

      // Preset Variables

      // Set Animation
      animation = new(sender,
        new(() => transform.localScale, value => transform.localScale = value),
        new(() => canvasGroup.alpha, value => canvasGroup.alpha = value))
      {
        minSize = new(0.8f, 0.8f),
        maxSize = transform.localScale,
        minAlpha = 0f,
        maxAlpha = 1f,
        fadeAnimSpeed = 6f,
        sizeAnimSpeed = 8f
      };

      animation.onStarted += _ => canvasGroup.blocksRaycasts = true;
      animation.onHid += _ => canvasGroup.blocksRaycasts = false;
      canvasGroup.alpha = 0f;
      canvasGroup.blocksRaycasts = false;
    }

    public override void Start() => Show();

    public override void Stop() => Hide();

    public void Show() => animation.Show();

    public void Hide() => animation.Hide();
  }
}
