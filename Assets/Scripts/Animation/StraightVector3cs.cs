using System;
using UnityEngine;
using Utils;

namespace Animation
{
  public class StraightVector3 : Smooth<StraightVector3, Vector3>
  {
    protected override LerpDelegate lerp => Vector3.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
    
    public StraightVector3(MonoBehaviour sender, StructPointer<Vector3> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
