using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
  public class Coroutiner
  {
    public MonoBehaviour sender { get; }

    public Func<IEnumerator> routine { get; }
    
    private Coroutine curCoroutine;

    public Coroutiner(MonoBehaviour sender, Func<IEnumerator> routine)
    {
      this.sender = sender;
      this.routine = routine;
    }

    public void Start()
    {
      Stop();
      curCoroutine = sender.StartCoroutine(routine.Invoke());
    }

    public void Stop()
    {
      if (curCoroutine is not null) 
        sender.StopCoroutine(curCoroutine);
    }
  }
}
