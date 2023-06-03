using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class SmoothFloat : Smooth<SmoothFloat, float>
  {
    protected override LerpDelegate lerp => Mathf.Lerp;
    protected override EqualDelegate endChecker => Mathf.Approximately;
    
    public SmoothFloat(MonoBehaviour sender, StructPointer<float> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
