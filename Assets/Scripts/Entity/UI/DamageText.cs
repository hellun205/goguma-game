using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Entity.UI
{
  public class DamageText : UIEntity
  {
    public static EntityType Type => EntityType.DamageText; 
    public override EntityType type => Type;

    [Header("UI Object")]
    [SerializeField]
    private TextMeshProUGUI tmp;

    private Coroutine fadeOut;

    public int damage
    {
      get => Int32.Parse(tmp.text);
      set => tmp.text = value.ToString();
    }

    public override void OnRelease()
    {
      base.OnRelease();

      if (fadeOut is not null)
        StopCoroutine(fadeOut);

      damage = 0;

      var color = tmp.color;
      color.a = 1f;
      tmp.color = color;
    }

    private IEnumerator FadeOut(float moveSpeed = 2.5f, float smoothing = 3f)
    {
      while (true)
      {
        var color = tmp.color;

        if (color.a > 0)
        {
          color.a = LerpUtils.ColorLerp(color.a, 0f, smoothing);
          tmp.color = color;
          Translate(0f, moveSpeed * Time.deltaTime);

          yield return new WaitForNextFrameUnit();
        }
        else
        {
          Release();
          yield break;
        }
      }
    }

    public void Show(int damage)
    {
      this.damage = damage;

      if (fadeOut is not null)
        StopCoroutine(fadeOut);

      fadeOut = StartCoroutine(FadeOut());
    }
  }
}
