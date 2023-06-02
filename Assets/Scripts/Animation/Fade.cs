using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public abstract class Fade<T> : Lerper<T, Color> where T : Fade<T>
  {
    private const float fadeInValue = 1f;
    private const float fadeOutValue = 0f;
    
    protected Func<Color> originalColor;

    protected Fade(MonoBehaviour sender, Func<Color> colorPointer, float startValue, Action<Color> onValueChanged) :
      base(sender, colorPointer.Invoke().Setter(a: startValue), onValueChanged)
    {
      originalColor = colorPointer;
    }

    protected override LerpDelegate lerp => (a, b, t) =>
    {
      var color = originalColor.Invoke();
      return new Color(color.r, color.g, color.b, Mathf.Lerp(a.a, b.a, t));
    };

    protected override EqualDelegate endChecker => (a, b) => Mathf.Approximately(a.a, b.a);

    public void FadeIn(float speed) => Start(fadeInValue, speed);

    public void FadeOut(float speed) => Start(fadeOutValue, speed);

    public void Start(float endAlpha, float speed)
    {
      var color = originalColor.Invoke();
      Start(color, color.Setter(a: endAlpha), speed);
    }
  }
}
