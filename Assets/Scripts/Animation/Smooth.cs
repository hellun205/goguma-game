using System;
using System.Collections;
using UnityEngine;

namespace Animation
{
  public abstract class Smooth<T, TValue> : Lerper<T, TValue> where T : Smooth<T, TValue>
  {
    public Smooth(MonoBehaviour sender, TValue startValue, Action<TValue> onValueChanged) :
      base(sender, startValue, onValueChanged)
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
