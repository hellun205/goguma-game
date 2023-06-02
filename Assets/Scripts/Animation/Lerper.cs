using System;
using System.Collections;
using UnityEngine;

namespace Animation
{
  public abstract class Lerper<T, TValue> : BaseAnimation<T, TValue> where T : Lerper<T,TValue>
  {
    protected abstract LerpDelegate lerp { get; }
    
    protected abstract EqualDelegate endChecker { get; }
    
    protected Lerper(MonoBehaviour sender, TValue startValue, Action<TValue> onValueChanged)
      : base(sender, startValue, onValueChanged)
    {
    }
  }
}
