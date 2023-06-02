using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  [Obsolete("has bug",true)]
  public sealed class SmoothFade : Fade<SmoothFade>
  {
    public SmoothFade(MonoBehaviour sender, Func<Color> colorPointer, float startValue, Action<Color> onValueChanged) :
      base(sender, colorPointer, startValue, onValueChanged)
    {
    }

    protected override IEnumerator Routine()
    {
      value = startValue;

      while (!endChecker.Invoke(value, endValue) && !isTimeOut)
      {
        yield return new WaitForEndOfFrame();
        value = lerp.Invoke(value, endValue, time);
        SpendTime();
      }

      CallEndedEvent();
    }
  }
}
