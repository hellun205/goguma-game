using System;
using System.Collections;
using UnityEngine;

namespace Animation
{
  public class StraightFloat : Straight<StraightFloat, float>
  {
    public StraightFloat(MonoBehaviour sender, float startValue, Action<float> onValueChanged) :
      base(sender, startValue, onValueChanged)
    {
    }

    protected override LerpDelegate lerp => Mathf.Lerp;
    protected override EqualDelegate endChecker => Mathf.Approximately;
  }
}
