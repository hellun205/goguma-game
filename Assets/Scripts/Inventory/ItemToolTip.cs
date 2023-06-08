﻿using Entity.Item;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{
  public class ItemToolTip : MonoBehaviour
  {
    [HideInInspector]
    public BaseItem itemData;

    [SerializeField]
    private TextMeshProUGUI text;

    public void Refresh()
    {
      text.SetText(itemData.GetTooltipText());
    }
  }
}
