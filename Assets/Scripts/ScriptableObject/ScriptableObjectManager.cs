using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObject
{
  public class ScriptableObjectManager<T> : MonoBehaviour where T : UnityEngine.ScriptableObject
  {
    public static ScriptableObjectManager<T> Instance { get; private set; }
    [SerializeField]
    private T[] list;

    public HashSet<T> items { get; private set; }

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      else
        Destroy(this);

      items = list.ToHashSet();
      list = null;
    }

    public T GetWithCode(string code) => Get(item => item.name == code).Single();

    public IEnumerable<T> Get(Predicate<T> predicate) => items.Where(items => predicate(items));

    private void Start()
    {
    }
  }
}