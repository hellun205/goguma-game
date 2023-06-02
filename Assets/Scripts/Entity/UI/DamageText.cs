﻿using System;
using System.Collections;
using Animation;
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

    // Animation
    private SmoothVector2 animSizeUp;
    private SmoothVector2 animSizeDown;
    private SmoothFloat animFade;
    private bool isIn = true;
    
    private static readonly Vector2 NormalSize = new Vector2(3f, 3f);
    private static readonly Vector2 MinSize = new Vector2(0.1f, 0.1f);

    public int damage
    {
      get => Int32.Parse(tmp.text);
      set => tmp.text = value.ToString();
    }

    protected override void Awake()
    {
      base.Awake();
      animSizeUp = new SmoothVector2(this, MinSize, value => transform.localScale = value);
      animSizeDown = new SmoothVector2(this, NormalSize, value => transform.localScale = value);
      animFade = new SmoothFloat(this, 0f, value =>
      {
        var color = tmp.color;
        color.a = value;
        tmp.color = color;
      });
      
      animSizeUp.timeout = 0.2f;
      animSizeDown.timeout = 0.5f;
      animFade.timeout = 0.6f;
      
      animSizeUp.onEnded += AnimSizeUpOnonEnded;
      animFade.onEnded += AnimFadeOnonEnded;
    }

    public override void OnRelease()
    {
      base.OnRelease();
    }

    public void Show(int damage)
    {
      this.damage = damage;
      isIn = true;
      animSizeUp.Start(MinSize, NormalSize, 13f);
      animFade.Start(0f, 1f, 5f);
    }
    
    private void AnimSizeUpOnonEnded(SmoothVector2 sender)
    {
      animSizeDown.Start(transform.localScale, MinSize, 7f);
      isIn = false;
      animFade.Start(1f, 0f, 7f);
    }
    
    private void AnimFadeOnonEnded(SmoothFloat sender)
    {
      if (!isIn)
        Release();  
    }
  }
}
