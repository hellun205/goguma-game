using System;
using Entity.Item;
using Entity.Item.Useable;
using Entity.Player;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using Utils;

namespace Buff
{
  public class Buff : MonoBehaviour, IPointerClickHandler
  {
    [Header("UI Object")]
    [SerializeField]
    private Image iconImg;

    [SerializeField]
    private TextMeshProUGUI leftTimeTMP;

    [SerializeField]
    private Slider slider;

    [Header("BuffItem")]
    public float time;

    public float endTime;

    public BuffItem item;

    public bool isEnabled;

    private void Update()
    {
      if (!isEnabled) return;

      if (time < endTime)
        time += Time.deltaTime;
      else
        End();

      slider.value = time / endTime;
      leftTimeTMP.text = (endTime - time).GetTimeStr();
    }

    public void Set(BuffItem item)
    {
      this.item = item;
      this.endTime = item.time;
      time = 0f;
      iconImg.sprite = item.sprite8x;
      PlayerController.Instance.status += item.increase;
      isEnabled = true;
    }

    private void End()
    {
      isEnabled = false;
      PlayerController.Instance.status -= item.increase;
      BuffManager.Instance.Remove(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button == PointerEventData.InputButton.Right)
        End();
    }
  }
}
