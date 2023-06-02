using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public class StraightVector2 : Smooth<StraightVector2, Vector2>
  {
    public StraightVector2(MonoBehaviour sender, Vector2 startValue, Action<Vector2> onValueChanged) :
      base(sender, startValue, onValueChanged)
    {
    }

    protected override LerpDelegate lerp => Vector2.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
  }
}
