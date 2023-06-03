using UnityEngine;
using Utils;

namespace Animation
{
  public class StraightVector2 : Smooth<StraightVector2, Vector2>
  {
    protected override LerpDelegate lerp => Vector2.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
    
    public StraightVector2(MonoBehaviour sender, StructPointer<Vector2> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
