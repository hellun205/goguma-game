using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class StraightFade : Fade<StraightFade>
  {
    public StraightFade(MonoBehaviour sender, Func<Color> colorPointer, float startValue, Action<Color> onValueChanged) :
      base(sender, colorPointer, startValue, onValueChanged)
    {
    }

    protected override IEnumerator Routine()
    {
      value = startValue;
      var timer = 0f;
      
      while (!endChecker.Invoke(value, endValue))
      {
        yield return new WaitForEndOfFrame();
        timer += time;
        value = lerp.Invoke(startValue, endValue, timer);
      }

      CallEndedEvent();
    }
  }
}
