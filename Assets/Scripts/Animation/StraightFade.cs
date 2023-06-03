using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class StraightFade : Fade<StraightFade>
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
    
    public StraightFade(MonoBehaviour sender, StructPointer<Color> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
