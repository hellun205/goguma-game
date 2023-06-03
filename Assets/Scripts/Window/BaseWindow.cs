using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Window
{
  public abstract class BaseWindow : MonoBehaviour
  {
    [Header("UI Object - Base Window")]
    [SerializeField]
    protected TextMeshProUGUI titleTMP;

    [SerializeField]
    protected Button closeBtn;

    [FormerlySerializedAs("_title")]
    [Header("Window")]
    public string title;

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
