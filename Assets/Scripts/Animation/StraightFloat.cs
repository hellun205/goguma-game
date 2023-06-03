using UnityEngine;
using Utils;

namespace Animation
{
  public class StraightFloat : Straight<StraightFloat, float>
  {
    protected override LerpDelegate lerp => Mathf.Lerp;
    protected override EqualDelegate endChecker => Mathf.Approximately;
    
    public StraightFloat(MonoBehaviour sender, StructPointer<float> valuePointer) : base(sender, valuePointer)
    {
    }
  }
}
