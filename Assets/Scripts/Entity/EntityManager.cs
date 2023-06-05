using System;
using System.Collections.Generic;
using Entity.UI;
using Manager;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity
{
  public class EntityManager : SingleTon<EntityManager>
  {
    /// <summary>
    /// Container of entity
    /// </summary>
    public Transform ContainerOfEntity { get; private set; }

    /// <summary>
    /// Container of ui type entity
    /// </summary>
    public Transform ContainerOfUIEntity { get; private set; }

    /// <summary>
    /// Object Pool
    /// </summary>
    private Dictionary<string, IObjectPool<Entity>> pools;

    public delegate void entityEvent(Entity entity);

    public delegate void defaultEvent();

    public event entityEvent onReleaseBefore;

    public event entityEvent onGetAfter;

    public Vector2 getPosition = Vector2.zero;

    public Vector2 releasePosition = Vector2.zero;

    protected override void Awake()
    {
      base.Awake();
      ContainerOfEntity = GameObject.FindWithTag("EntityCollection").transform;
      ContainerOfUIEntity = GameObject.FindWithTag("UIEntityCollection").transform;

      pools = new Dictionary<string, IObjectPool<Entity>>();
    }
    
    private Entity OnCreateObject(string type)
    {
      var prefab = Managers.Prefab.GetObject<Entity>(type);
      var parent = prefab is UIEntity ? ContainerOfUIEntity : ContainerOfEntity;
      var obj = Instantiate(prefab, parent);
      obj.name = type;
      
      return obj;
    }
    
    private void OnGetObject(Entity entity)
    {
      entity.position = getPosition;
      entity.gameObject.SetActive(true);
      onGetAfter?.Invoke(entity);
      entity.OnGet();
    }
    
    private void OnReleaseObject(Entity entity)
    {
      onReleaseBefore?.Invoke(entity);
      entity.OnRelease();
      entity.gameObject.SetActive(false);
      entity.position = releasePosition;
    }
    
    private void OnDetroyObject(Entity entity) => Destroy(entity.gameObject);

    /// <summary>
    /// Get entity from pool
    /// </summary>
    /// <param name="startPosition">entity's position</param>
    /// <param name="set">setting entity</param>
    /// <returns>entity</returns>
    public T GetEntity<T>(Vector2 startPosition, Action<T> set = null) where T : Entity
    {
      var type = typeof(T).Name;
      if (!pools.ContainsKey(type))
      {
        pools.Add(type, new ObjectPool<Entity>(
          () => OnCreateObject(type),
          OnGetObject,
          OnReleaseObject,
          OnDetroyObject
        ));
      }

      getPosition = startPosition;
      var entity = pools[type].Get();
      set?.Invoke((T) entity);

      return (T) entity;
    }

    /// <summary>
    /// Release entity
    /// </summary>
    /// <param name="entity">entity to release</param>
    public void ReleaseEntity(Entity entity) => pools[entity.name].Release(entity);
  }
}
