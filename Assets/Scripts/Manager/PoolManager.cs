using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Manager
{
  public abstract class PoolManager<TObject> : SingleTon<PoolManager<TObject>> where TObject : Component
  {
    public delegate void PoolEventListener(TObject sender);

    public event PoolEventListener onGet;
    public event PoolEventListener onRelease;

    protected Dictionary<string, IObjectPool<TObject>> pools = new Dictionary<string, IObjectPool<TObject>>();

    protected Transform parent;

    public T Get<T>([CanBeNull] Action<T> objSet = null) where T : Component
    {
      var type = typeof(T).Name;
      if (!pools.ContainsKey(type))
      {
        pools.Add(type, new ObjectPool<TObject>(() => OnCreatePool(type), OnGetPool, OnReleasePool, OnDestroyPool));
      }

      var obj = pools[type].Get();
      var objT = obj as T;
      objSet?.Invoke(objT);
      onGet?.Invoke(obj);
      return objT;
    }

    public void Release<T>(T obj) where T : Component
    {
      var type = typeof(T).Name;
      if (!pools.ContainsKey(type))
      {
        Debug.LogError($"Can't release object. This is not managed by this manager.");
        return;
      }

      var objT = obj as TObject;
      pools[type].Release(objT);
      onRelease?.Invoke(objT);
    }

    protected virtual TObject OnCreatePool(string type) =>
      Instantiate(Managers.Prefab.GetObject<TObject>(type), parent);

    protected virtual void OnGetPool(TObject obj) => obj.gameObject.SetActive(true);

    protected virtual void OnReleasePool(TObject obj) => obj.gameObject.SetActive(false);

    protected virtual void OnDestroyPool(TObject obj) => Destroy(obj.gameObject);
  }
}
