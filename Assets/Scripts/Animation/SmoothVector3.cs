using UnityEngine;
using Utils;

namespace Animation
{
  public sealed class SmoothVector3 : Smooth<SmoothVector3, Vector3>
  {
    protected override LerpDelegate lerp => Vector3.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
    public SmoothVector3(MonoBehaviour sender, StructPointer<Vector3> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
