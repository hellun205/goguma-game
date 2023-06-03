using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class SmoothFade : Fade<SmoothFade>
  {
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
    
    public SmoothFade(MonoBehaviour sender, StructPointer<Color> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
