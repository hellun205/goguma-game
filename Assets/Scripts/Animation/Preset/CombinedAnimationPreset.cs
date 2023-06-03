using Animation.Combined;
using UnityEngine;

namespace Animation.Preset
{
  public abstract class CombinedAnimationPreset<T> where T : CombinedAnimation<T>
  {
    public T animation { get; protected set; }
    protected MonoBehaviour sender { get; }

    protected CombinedAnimationPreset(MonoBehaviour sender)
    {
      this.sender = sender;
    }

    public abstract void Start();
    public abstract void Stop();
  }
}
