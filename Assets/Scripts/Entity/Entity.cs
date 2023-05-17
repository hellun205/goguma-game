using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity {
  public abstract class Entity : MonoBehaviour {
    public string name;
    public abstract EntityType type { get; }

    public virtual Vector3 position {
      get => transform.position;
      set => transform.position = value;
    }

    // private void OnBecameInvisible() => Release();

    public void Release() => EntityManager.Release(this);
    
  }
}