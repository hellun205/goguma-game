using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class SmoothVector3 : Smooth<SmoothVector3, Vector3>
  {
    public SmoothVector3(MonoBehaviour sender, Vector3 startValue, Action<Vector3> onValueChanged) :
      base(sender, startValue, onValueChanged)
    {
    }

    protected override LerpDelegate lerp => Vector3.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
  }
}
