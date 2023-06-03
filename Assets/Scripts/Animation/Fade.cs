using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public abstract class Fade<T> : Lerper<T, Color> where T : Fade<T>
  {
    private const float fadeInValue = 1f;
    private const float fadeOutValue = 0f;

    protected override LerpDelegate lerp => (a, b, t) => value.Setter(a: Mathf.Lerp(a.a, b.a, t));

    protected override EqualDelegate endChecker => (a, b) => Mathf.Approximately(a.a, b.a);

    public void FadeIn(float speed) => Start(fadeInValue, speed);

    public void FadeOut(float speed) => Start(fadeOutValue, speed);

    public void Start(float endAlpha, float speed)
    {
      var color = value;
      Start(color, color.Setter(a: endAlpha), speed);
    }
    protected Fade(MonoBehaviour sender, StructPointer<Color> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
