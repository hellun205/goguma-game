using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public abstract class Smooth<T, TValue> : Lerper<T, TValue> where T : Smooth<T, TValue>where TValue : struct
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
    
    protected Smooth(MonoBehaviour sender, StructPointer<TValue> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
