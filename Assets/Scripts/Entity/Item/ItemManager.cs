using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Item {
  public class ItemManager : MonoBehaviour {
    [SerializeField]
    private Item[] list;
    
    public HashSet<Item> items { get; private set; }

    private void Awake() {
      items = list.ToHashSet();
      list = null;
    }

    public Item GetWithCode(string code) => Get(item => item.name == code).Single();

    public IEnumerable<Item> Get(Predicate<Item> predicate) => items.Where(items => predicate(items));

    private void Start() {
      
    }
  }
}