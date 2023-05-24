using System.Collections.Generic;
using System.Linq;
using Entity.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity {
  /// <summary>
  /// 엔티티를 설정하고, 오브젝트 풀을 관리할 매니져 입니다. (싱글톤)
  /// </summary>
  public class EntityManager : MonoBehaviour {
    /// <summary>
    /// 현재 인스턴스를 가져옵니다
    /// </summary>
    public static EntityManager Instance { get; protected set; }
    
    /// <summary>
    /// 엔티티 풀 관리에 대한 데이터를 가져옵니다.
    /// </summary>
    public EntityPoolManageData[] managements;

    /// <summary>
    /// 엔티티 객체를 담을 부모 객체를 가져옵니다.
    /// </summary>
    public Transform entityCollection;
    
    /// <summary>
    /// UI 형식의 엔티티 객체를 담을 부모 객체를 가져옵니다.
    /// </summary>
    public RectTransform uiEntityCollection;

    /// <summary>
    /// 오브젝트 풀 입니다.
    /// </summary>
    private Dictionary<EntityType, IObjectPool<Entity>> pools;
    
    public delegate void entityEvent(Entity entity);
    public delegate void defaultEvent();
    
    public event entityEvent onReleaseBefore;

    public event entityEvent onReleaseAfter;
    
    public event defaultEvent onGetBefore;
    
    public event entityEvent onGetAfter;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(this);
      // DontDestroyOnLoad(gameObject);
      // DontDestroyOnLoad(entityCollection.gameObject);
      // DontDestroyOnLoad(uiEntityCollection.gameObject);
      
      var list = managements.Select(x => x.type);

      pools = new Dictionary<EntityType, IObjectPool<Entity>>();
      foreach (var type in list) {
        var data = managements.Where(x => x.type == type).Single();

        pools.Add(type, null);
        pools[type] = new ObjectPool<Entity>(() => OnCreateObject(data), OnGetObject, OnReleaseObject,
          OnDetroyObject, maxSize: data.maxCount);
      }
    }

    /// <summary>
    /// 풀을 생성할 때 실행됩니다.
    /// </summary>
    /// <param name="data">엔티티 풀 관리에 대한 데이터</param>
    /// <returns>만들어진 객체</returns>
    private Entity OnCreateObject(EntityPoolManageData data) {
      var parent = data.isUI ? uiEntityCollection : entityCollection;
      var obj = Instantiate(data.prefab, parent);
      
      return obj;
    }

    /// <summary>
    /// 풀 객체를 가져올 때 실행됩니다.
    /// </summary>
    /// <param name="entity">엔티티</param>
    private void OnGetObject(Entity entity) => entity.gameObject.SetActive(true);

    /// <summary>
    /// 풀 객체를 비활성화할 때 실행됩니다.
    /// </summary>
    /// <param name="entity">엔티티</param>
    private void OnReleaseObject(Entity entity) => entity.gameObject.SetActive(false);

    /// <summary>
    /// 풀 객체를 삭제할 때 실행됩니다.
    /// </summary>
    /// <param name="entity">엔티티</param>
    private void OnDetroyObject(Entity entity) => Destroy(entity.gameObject);

    /// <summary>
    /// 풀에서 엔티티를 가져옵니다.
    /// </summary>
    /// <param name="type">가져올 엔티티 종류</param>
    /// <returns>가져온 엔티티</returns>
    public Entity GetEntity(EntityType type) {
      onGetBefore?.Invoke();
      var entity = pools[type].Get();
      onGetAfter?.Invoke(entity);
      entity.OnRelease();
      return entity;
      
    }

    /// <summary>
    /// 풀에서 엔티티를 비활성화(삭제) 합니다.
    /// </summary>
    /// <param name="entity">비활성화(삭제)할 엔티티</param>
    public void ReleaseEntity(Entity entity) {
      onReleaseBefore?.Invoke(entity);
      entity.OnRelease();
      pools[entity.type].Release(entity);
      onReleaseAfter?.Invoke(entity);
    }

    /// <summary>
    /// 엔티티를 생성합니다.
    /// </summary>
    /// <param name="type">생성할 엔티티 종류</param>
    /// <returns>생성된 엔티티</returns>
    public static Entity Get(EntityType type) => Instance.GetEntity(type);

    /// <summary>
    /// 엔티티를 삭제합니다.
    /// </summary>
    /// <param name="entity">삭제할 엔티티</param>
    public static void Release(Entity entity) => Instance.ReleaseEntity(entity);
    
  }
}