using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.UI
{
  public class UEText : UIEntity
  {
    [SerializeField]
    private TextMeshProUGUI textTMP;
    
    [SerializeField]
    private Image img;
    
    public string text
    {
      get => textTMP.text;
      set => textTMP.text = value;
    }
  }
}
