using System;
using Entity.UI;
using Manager;
using Pool;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity
{
  public class EntityManager : PoolManager<EntityManager, Entity>
  {
    /// <summary>
    /// Container of entity
    /// </summary>
    public Transform ContainerOfEntity { get; private set; }

    /// <summary>
    /// Container of ui type entity
    /// </summary>
    public Transform ContainerOfUIEntity { get; private set; }

    public delegate void entityEvent(Entity entity);

    public event entityEvent onReleaseEntityBefore;

    public event entityEvent onGetEntityAfter;

    public Vector2 getPosition = Vector2.zero;

    public Vector2 releasePosition = Vector2.zero;

    protected override void Awake()
    {
      base.Awake();

      ContainerOfEntity = GameObject.FindWithTag("EntityCollection").transform;
      ContainerOfUIEntity = GameObject.FindWithTag("UIEntityCollection").transform;
    }

    protected override Entity OnCreatePool(string type)
    {
      var prefab = Managers.Prefab.GetObject<Entity>(type);
      var parent = prefab is UIEntity ? ContainerOfUIEntity : ContainerOfEntity;
      var obj = Instantiate(prefab, parent);
      obj.name = type;

      return obj;
    }

    protected override void OnGetPool(Entity obj)
    {
      obj.position = getPosition;
      obj.gameObject.SetActive(true);
      onGetEntityAfter?.Invoke(obj);
      obj.OnGet();
    }

    protected override void OnReleasePool(Entity obj)
    {
      onReleaseEntityBefore?.Invoke(obj);
      obj.OnRelease();
      obj.gameObject.SetActive(false);
      obj.position = releasePosition;
    }

    /// <summary>
    /// Get entity from pool
    /// </summary>
    /// <param name="startPosition">entity's position</param>
    /// <param name="set">setting entity</param>
    /// <returns>entity</returns>
    public T Get<T>(Vector2 startPosition, Action<T> set = null) where T : Entity
    {
      getPosition = startPosition;
      return Get(set);
    }
    
    public void Release(Entity entity)
    {
      pools[entity.name].Release(entity);
    }
  }
}
