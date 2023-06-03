using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public abstract class Lerper<T, TValue> : BaseAnimation<T, TValue> where T : Lerper<T,TValue> where TValue : struct
  {
    protected abstract LerpDelegate lerp { get; }
    
    protected abstract EqualDelegate endChecker { get; }

    protected Lerper(MonoBehaviour sender, StructPointer<TValue> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
