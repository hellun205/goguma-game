using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Manager
{
  public abstract class ObjectCollector<T, TObject> : SingleTon<T>
    where TObject : UnityEngine.Object
    where T : ObjectCollector<T, TObject>
  {
    public List<TObject> items = new List<TObject>();

    public TObject GetObject(Predicate<TObject> predicate) => items.Single(predicate.Invoke);

    public TObject GetObject(string name) => GetObject(obj => obj.name == name);

    public TComponent GetObject<TComponent>(string name) where TComponent : Component
      => GetObject(name).GetComponent<TComponent>();
  }
}
