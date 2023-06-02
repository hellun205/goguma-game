using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public class StraightColor : Straight<StraightColor, Color>
  {
    public StraightColor(MonoBehaviour sender, Color startValue, Action<Color> onValueChanged) :
      base(sender, startValue, onValueChanged)
    {
    }

    protected override LerpDelegate lerp => Color.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
  }
}
