using System;
using System.Collections.Generic;
using Entity.Item.Useable;
using UnityEngine;
using UnityEngine.Pool;

namespace Buff
{
  public class BuffManager : MonoBehaviour
  {
    public static BuffManager Instance { get; private set; }

    private IObjectPool<Buff> pool;

    [SerializeField]
    private Buff prefab;

    [SerializeField]
    private Transform container;

    private List<BuffItem> list = new List<BuffItem>();

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      else
        Destroy(this);

      pool = new ObjectPool<Buff>(() => Instantiate(prefab, container),
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

    public void Remove(Buff buff)
    {
      list.Remove(buff.item);
      pool.Release(buff);
    }

    public bool HasBuff(BuffItem item) => list.Contains(item);
  }
}
