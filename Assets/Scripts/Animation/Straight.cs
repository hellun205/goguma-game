using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public abstract class Straight<T, TValue> : Lerper<T, TValue> where T : Straight<T, TValue>where TValue : struct
  {
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
    protected Straight(MonoBehaviour sender, StructPointer<TValue> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
