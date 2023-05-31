using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace Manager
{
  public class EntityManager : SingleTon<EntityManager>
  {
    /// <summary>
    /// 엔티티 풀 관리에 대한 데이터
    /// </summary>
    public EntityPoolManageData[] managements;

    /// <summary>
    /// Container of 
    /// </summary>
    public Transform ContainerOfEntity { get; private set; }

    /// <summary>
    /// Container of ui type entity
    /// </summary>
    public Transform ContainerOfUIEntity { get; private set; }

    /// <summary>
    /// 오브젝트 풀
    /// </summary>
    private Dictionary<EntityType, IObjectPool<Entity.Entity>> pools;

    public delegate void entityEvent(Entity.Entity entity);

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

      var list = managements.Select(x => x.type);

      pools = new Dictionary<EntityType, IObjectPool<Entity.Entity>>();

      foreach (var type in list)
      {
        var data = managements.Where(x => x.type == type).Single();

        pools.Add(type, null);
        pools[type] = new ObjectPool<Entity.Entity>(
          () => OnCreateObject(data),
          OnGetObject,
          OnReleaseObject,
          OnDetroyObject,
          maxSize: data.maxCount
        );
      }
    }

    /// <summary>
    /// Call on create pool obj
    /// </summary>
    /// <param name="data">Entity data</param>
    /// <returns>created object</returns>
    private Entity.Entity OnCreateObject(EntityPoolManageData data)
    {
      var parent = data.isUI ? ContainerOfUIEntity : ContainerOfEntity;
      var obj = Instantiate(data.prefab, parent);
      obj.transform.SetAsFirstSibling();

      return obj;
    }

    /// <summary>
    /// Call on get pool obj
    /// </summary>
    /// <param name="entity">entity to get</param>
    private void OnGetObject(Entity.Entity entity)
    {
      entity.position = getPosition;
      entity.gameObject.SetActive(true);
    }

    /// <summary>
    /// Call on release pool obj
    /// </summary>
    /// <param name="entity">entity to release</param>
    private void OnReleaseObject(Entity.Entity entity)
    {
      entity.gameObject.SetActive(false);
      entity.position = releasePosition;
    }

    /// <summary>
    /// Call on destroy pool obj
    /// </summary>
    /// <param name="entity">entity to destroy</param>
    private void OnDetroyObject(Entity.Entity entity) => Destroy(entity.gameObject);

    /// <summary>
    /// Get entity from pool
    /// </summary>
    /// <param name="type">entity type to get</param>
    /// <param name="startPosition">entity's position</param>
    /// <returns>entity</returns>
    public Entity.Entity GetEntity(EntityType type, Vector2 startPosition)
    {
      getPosition = startPosition;
      var entity = pools[type].Get();
      onGetAfter?.Invoke(entity);
      entity.OnGet();

      return entity;
    }

    /// <summary>
    /// Release entity
    /// </summary>
    /// <param name="entity">entity to release</param>
    public void ReleaseEntity(Entity.Entity entity)
    {
      onReleaseBefore?.Invoke(entity);
      entity.OnRelease();
      pools[entity.type].Release(entity);
    }

    public T GetEntity<T>(Vector2 startPosition) where T : Entity.Entity
      => (T) GetEntity(typeof(T).GetTypeProperty<EntityType>(), startPosition);

    public T GetEntity<T>(Vector2 startPosition, Action<T> entitySet) where T : Entity.Entity
    {
      var get = GetEntity<T>(startPosition);
      entitySet.Invoke(get);
      return get;
    }
  }
}
