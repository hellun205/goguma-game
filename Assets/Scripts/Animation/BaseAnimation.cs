using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Animation
{
  public abstract class BaseAnimation<T, TValue> where T : BaseAnimation<T, TValue>
  {
    protected delegate TValue LerpDelegate(TValue a, TValue b, float t);

    protected delegate bool EqualDelegate(TValue a, TValue b);
    
    public delegate void animationEventListener(T sender);

    public event animationEventListener onStarted;

    public event animationEventListener onEnded;

    protected MonoBehaviour sender { get; }

    public Coroutiner coroutiner { get; }

    private Action<TValue> onValueChanged;

    public bool isUnscaled { get; set; } = false;

    public TValue value
    {
      get => _value;
      protected set
      {
        _value = value;
        onValueChanged.Invoke(_value);
      }
    }

    private TValue _value;

    protected TValue startValue;
    protected TValue endValue;
    protected float speed;

    protected abstract IEnumerator Routine();

    protected BaseAnimation(MonoBehaviour sender, TValue startValue, Action<TValue> onValueChanged)
    {
      this.onValueChanged = onValueChanged;
      _value = startValue;
      this.sender = sender;
      coroutiner = new Coroutiner(sender, Routine);
    }

    protected void Start(TValue start, TValue end, float speed)
    {
      this.speed = speed;
      coroutiner.Start();
      CallStartedEvent();
    }

    protected void CallStartedEvent() => onStarted?.Invoke((T) this);
    
    protected void CallEndedEvent() => onEnded?.Invoke((T) this);

    protected float time => (isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime) * speed;
  }
}
