using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class SmoothVector2 : Smooth<SmoothVector2, Vector2>
  {
    protected override LerpDelegate lerp => Vector2.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
    
    public SmoothVector2(MonoBehaviour sender, StructPointer<Vector2> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
