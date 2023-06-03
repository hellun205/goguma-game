using UnityEngine;

namespace Animation.Combined
{
  public class CombinedAnimation<T> where T : CombinedAnimation<T>
  {
    public delegate void CombinedAnimationEventListener(T sender);

    public event CombinedAnimationEventListener onStarted;
    public event CombinedAnimationEventListener onEnded;
    
    protected MonoBehaviour sender { get; }

    protected CombinedAnimation(MonoBehaviour sender)
    {
      this.sender = sender;
    }

    protected void CallStartEvent() => onStarted?.Invoke((T)this);
    protected void CallEndedEvent() => onEnded?.Invoke((T)this);
  }
}
