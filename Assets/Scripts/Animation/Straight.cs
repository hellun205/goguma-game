using System;
using System.Collections;
using UnityEngine;

namespace Animation
{
  public abstract class Straight<T, TValue> : BaseAnimation<T, TValue> where T : Straight<T, TValue>
  {
    protected abstract LerpDelegate lerp { get; }
    
    protected abstract EqualDelegate endChecker { get; }
    
    protected Straight(MonoBehaviour sender, TValue startValue, Action<TValue> onValueChanged) :
      base(sender, startValue, onValueChanged)
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
