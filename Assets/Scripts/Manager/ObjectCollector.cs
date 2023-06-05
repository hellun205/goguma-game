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

    public TObject GetObject(Predicate<TObject> predicate)
    {
      var obj = items.SingleOrDefault(predicate.Invoke);
      if (obj is null) Debug.LogError($"Can't find {typeof(TObject).Name} object");
      return obj;
    }

    public TObject GetObject(string name) => GetObject(obj => obj.name == name);

    public TComponent GetObject<TComponent>(string name) where TComponent : Component
      => GetObject(name).GetComponent<TComponent>();

    public int GetID(TObject obj) => items.IndexOf(obj);

    public TObject GetObjectByID(int id) => items[id];
  }
}
