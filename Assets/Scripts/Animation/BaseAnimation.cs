using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public abstract class BaseAnimation<T, TValue> where T : BaseAnimation<T, TValue> where TValue : struct
  {
    protected delegate TValue LerpDelegate(TValue a, TValue b, float t);

    protected delegate bool EqualDelegate(TValue a, TValue b);
    
    public delegate void animationEventListener(T sender);

    public event animationEventListener onStarted;

    public event animationEventListener onEnded;

    protected MonoBehaviour sender { get; }

    public Coroutiner coroutiner { get; }

    protected StructPointer<TValue> pointer { get; }

    public bool isUnscaled { get; set; } = false;

    public float timeout { get; set; } = 10f;
    
    protected float currentTimeout;

    public TValue value
    {
      get => pointer.value;
      protected set => pointer.value = value;
    }
    
    protected TValue startValue;
    
    protected TValue endValue;
    
    protected float speed;

    protected abstract IEnumerator Routine();

    protected BaseAnimation(MonoBehaviour sender, StructPointer<TValue> valuePointer)
    {
      pointer = valuePointer;
      this.sender = sender;
      coroutiner = new Coroutiner(sender, Routine);
    }

    public void Start(TValue start, TValue end, float speed)
    {
      this.speed = speed;
      startValue = start;
      endValue = end;
      currentTimeout = 0f;
      coroutiner.Start();
      CallStartedEvent();
    }

    protected void CallStartedEvent() => onStarted?.Invoke((T) this);
    
    protected void CallEndedEvent() => onEnded?.Invoke((T) this);

    protected float time => (isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime) * speed;

    protected bool isTimeOut => currentTimeout >= timeout;

    protected void SpendTime() => currentTimeout += Time.unscaledDeltaTime;
  }
}
