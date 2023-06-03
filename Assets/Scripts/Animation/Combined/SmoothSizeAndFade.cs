using System;
using UnityEngine;
using Utils;

namespace Animation.Combined
{
  public sealed class SmoothSizeAndFade : CombinedAnimation<SmoothSizeAndFade>
  {
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
    private SmoothFloat animFade;

    private bool isFadeIn = true;

    public SmoothSizeAndFade
    (
      MonoBehaviour sender,
      StructPointer<Vector3> sizePointer,
      StructPointer<float> alphaPointer
    ) : base(sender)
    {
      size = sizePointer;
      alpha = alphaPointer;
      animSize = new SmoothVector3(sender, size);
      animFade = new SmoothFloat(sender, alpha);

      animFade.onEnded += OnFadeEnded;

      _tmpZ = size.value.z;
    }

    public void Show()
    {
      isFadeIn = true;
      CallStartEvent();
      animSize.Start(minSize, maxSize, sizeAnimSpeed);
      animFade.Start(minAlpha, maxAlpha, fadeAnimSpeed);
    }

    public void Hide()
    {
      isFadeIn = false;
      animSize.Start(size.value, minSize, sizeAnimSpeed);
      animFade.Start(alpha.value, minAlpha, fadeAnimSpeed);
    }

    private void OnFadeEnded(SmoothFloat smoothFloat)
    {
      if (isFadeIn) return;
      CallEndedEvent();
    }
  }
}
