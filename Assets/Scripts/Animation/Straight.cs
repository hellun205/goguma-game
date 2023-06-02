using System;
using System.Collections;
using UnityEngine;

namespace Animation
{
  public abstract class Straight<T, TValue> : Lerper<T, TValue> where T : Straight<T, TValue>
  {
    protected Straight(MonoBehaviour sender, TValue startValue, Action<TValue> onValueChanged) :
      base(sender, startValue, onValueChanged)
    {
    }
    
    protected override IEnumerator Routine()
    {
      value = startValue;
      var timer = 0f;
      
      while (!endChecker.Invoke(value, endValue) && !isTimeOut)
      {
        yield return new WaitForEndOfFrame();
        timer += time;
        value = lerp.Invoke(startValue, endValue, timer);
        SpendTime();
      }

      CallEndedEvent();
    }
  }
}
