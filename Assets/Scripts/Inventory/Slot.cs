using System;
using Entity.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
  public class Slot : MonoBehaviour {
    [HideInInspector]
    public Item item;
    
    [HideInInspector]
    public byte count = 1;

    [Header("UI Object")]
    [SerializeField]
    private Image iconImg;
    
    [SerializeField]
    private TextMeshProUGUI countTMP;

    public void SetItem(Item item, byte count = 1) {
      this.item = item;
      this.count = count;

      iconImg.sprite = item.sprite;
      iconImg.color = item.spriteColor;
      countTMP.text = (count == 1 ? string.Empty : count.ToString());
    }
  }
}