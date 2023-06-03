using UnityEngine;
using Utils;

namespace Animation.Combined
{
  public sealed class SmoothSizeAndFade : CombinedAnimation<SmoothSizeAndFade>
  {
    public event CombinedAnimationEventListener onShowed;
    public event CombinedAnimationEventListener onHid;

    public Vector2 minSize = Vector2.zero;
    public Vector2 maxSize = Vector2.one;
    public float minAlpha = 0f;
    public float maxAlpha = 1f;

    public float sizeAnimSpeed = 1f;
    public float fadeAnimSpeed = 1f;

    private float _tmpZ;

    private StructPointer<Vector3> size;
    private StructPointer<float> alpha;

    private SmoothVector3 animSize;
    private StraightFloat animFade;

    private bool isFadeIn = true;

    public override float timeOut
    {
      get => animSize.timeout;
      set
      {
        animSize.timeout = value;
        animFade.timeout = value;
      }
    }

    public override bool isUnscaled
    {
      get => animSize.isUnscaled;
      set
      {
        animSize.isUnscaled = value;
        animFade.isUnscaled = value;
      }
    }

    public SmoothSizeAndFade
    (
      MonoBehaviour sender,
      StructPointer<Vector3> sizePointer,
      StructPointer<float> alphaPointer
    ) : base(sender)
    {
      size = sizePointer;
      alpha = alphaPointer;
      animSize = new(sender, size);
      animFade = new(sender, alpha);
      timeOut = 1f;

      animSize.onEnded += OnSizeEnded;
      animFade.onEnded += OnFadeEnded;

      _tmpZ = size.value.z;
    }

    private void OnSizeEnded(SmoothVector3 smoothVector3)
    {
      if (isFadeIn) onShowed?.Invoke(this);
    }

    public void Show()
    {
      isFadeIn = true;
      CallStartEvent();
      animSize.Start(ToVector3(minSize), ToVector3(maxSize), sizeAnimSpeed);
      animFade.Start(minAlpha, maxAlpha, fadeAnimSpeed);
    }

    public void Hide()
    {
      isFadeIn = false;
      animSize.Start(ToVector3(size.value), ToVector3(minSize), sizeAnimSpeed);
      animFade.Start(alpha.value, minAlpha, fadeAnimSpeed);
    }

    private void OnFadeEnded(StraightFloat smoothFloat)
    {
      CallEndedEvent();
      if (!isFadeIn) onHid?.Invoke(this);
    }

    private Vector3 ToVector3(Vector2 vector2) => ((Vector3) vector2).Setter(z: _tmpZ);
  }
}
