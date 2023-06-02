using System;
using System.Collections;
using UnityEngine;

namespace Animation
{
  public abstract class Smooth<T, TValue> : BaseAnimation<T, TValue> where T : Smooth<T, TValue>
  {
    protected abstract LerpDelegate lerp { get; }
    
    protected abstract EqualDelegate endChecker { get; }

    public Smooth(MonoBehaviour sender, TValue startValue, Action<TValue> onValueChanged) :
      base(sender, startValue, onValueChanged)
    {
    }

    protected override IEnumerator Routine()
    {
      value = startValue;

      while (!endChecker.Invoke(value, endValue))
      {
        yield return new WaitForEndOfFrame();
        value = lerp.Invoke(value, endValue, time);
      }

      CallEndedEvent();
    }
  }
}
