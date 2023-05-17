using System;
using System.Collections.Generic;
using System.Linq;
using Entity.UI;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace Entity {
  public class EntityManager : MonoBehaviour {
    public static EntityManager Instance { get; protected set; }
    
    public PoolManageData[] managements;

    public Transform entityCollection;
    
    public RectTransform uiEntityCollection;

    private Dictionary<EntityType, IObjectPool<Entity>> pools;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
      DontDestroyOnLoad(entityCollection.gameObject);
      DontDestroyOnLoad(uiEntityCollection.gameObject);
      
      var list = managements.Select(x => x.type);

      pools = new Dictionary<EntityType, IObjectPool<Entity>>();
      foreach (var type in list) {
        var data = managements.Where(x => x.type == type).Single();

        pools.Add(type, null);
        pools[type] = new ObjectPool<Entity>(() => OnCreateObject(data), OnGetObject, OnReleaseObject,
          OnDetroyObject, maxSize: data.maxCount);
      }
    }

    private Entity OnCreateObject(PoolManageData data) {
      var obj = Instantiate(data.prefab);
      
      obj.transform.SetParent(obj is UIEntity ? uiEntityCollection : entityCollection);
      return obj;
    }

    private void OnGetObject(Entity entity) => entity.gameObject.SetActive(true);

    private void OnReleaseObject(Entity entity) => entity.gameObject.SetActive(false);

    private void OnDetroyObject(Entity entity) => Destroy(entity.gameObject);

    public Entity GetEntity(EntityType type) => pools[type].Get();

    public void ReleaseEntity(Entity entity) => pools[entity.type].Release(entity);

    public static Entity Get(EntityType type) => Instance.GetEntity(type);

    public static void Release(Entity entity) => Instance.ReleaseEntity(entity);
    
  }
}