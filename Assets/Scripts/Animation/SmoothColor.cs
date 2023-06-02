using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class SmoothColor : Smooth<SmoothColor, Color>
  {
    public SmoothColor(MonoBehaviour sender, Color startValue, Action<Color> onValueChanged) :
      base(sender, startValue, onValueChanged)
    {
    }

    protected override LerpDelegate lerp => Color.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
  }
}
