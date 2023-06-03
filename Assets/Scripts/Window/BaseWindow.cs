using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Window
{
  public abstract class BaseWindow : MonoBehaviour
  {
    [Header("Window - Object")]
    [SerializeField]
    protected TextMeshProUGUI titleTMP;

    [SerializeField]
    protected Button closeBtn;

    [Header("Window")]
    public string title;

    protected bool interactable = true;

    protected virtual void Awake()
    {
      closeBtn.onClick.AddListener(OnCloseButtonClick);
    }

    protected abstract void OnCloseButtonClick();

    protected virtual void OnValidate()
    {
      titleTMP.text = title;
    }
  }
}
