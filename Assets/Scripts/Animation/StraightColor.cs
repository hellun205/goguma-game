using UnityEngine;
using Utils;

namespace Animation
{
  public class StraightColor : Straight<StraightColor, Color>
  {
    protected override LerpDelegate lerp => Color.Lerp;
    protected override EqualDelegate endChecker => (a, b) => a.IsEqual(b);
    
    public StraightColor(MonoBehaviour sender, StructPointer<Color> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
