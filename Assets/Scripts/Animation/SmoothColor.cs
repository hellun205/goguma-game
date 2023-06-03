using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class SmoothColor : Smooth<SmoothColor, Color>
  {
    protected override LerpDelegate lerp => Color.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
    
    public SmoothColor(MonoBehaviour sender, StructPointer<Color> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
