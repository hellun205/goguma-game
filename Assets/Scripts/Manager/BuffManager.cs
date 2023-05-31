using System.Collections.Generic;
using Entity.Item.Useable;
using UnityEngine;
using UnityEngine.Pool;

namespace Manager
{
  public class BuffManager : SingleTon<BuffManager>
  {
    private IObjectPool<Buff.Buff> pool;

    [SerializeField]
    private Buff.Buff prefab;

    [SerializeField]
    private Transform container;

    private List<BuffItem> list = new List<BuffItem>();

    protected override void Awake()
    {
      base.Awake();
      
      pool = new ObjectPool<Buff.Buff>(() => Instantiate(prefab, container),
        buff => buff.gameObject.SetActive(true),
        buff => buff.gameObject.SetActive(false),
        buff => Destroy(buff.gameObject));
    }

    public void Add(BuffItem item)
    {
      var buff = pool.Get();
      list.Add(item);
      buff.Set(item);
    }

    public void Remove(Buff.Buff buff)
    {
      list.Remove(buff.item);
      pool.Release(buff);
    }

    public bool HasBuff(BuffItem item) => list.Contains(item);
  }
}
