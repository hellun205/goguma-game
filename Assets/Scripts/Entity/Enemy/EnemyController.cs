using System;
using System.Collections;
using Animation;
using Entity.UI;
using Manager;
using UnityEngine;

namespace Entity.Enemy
{
  public class EnemyController : Entity
  {
    public static EntityType Type => EntityType.Enemy;
    public override EntityType type => Type;

    public EnemyStatus status;

    [SerializeField]
    protected Color hitColor = Color.red;

    protected Color defaultColor;

    protected bool canHit = true;

    [SerializeField]
    protected string deadAnimParam = "isDead";

    [SerializeField]
    protected float knockBackPower = 50f;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    protected Rigidbody2D rigid;

    protected Animator anim;

    protected HpBar hpBar;

    // Animation
    protected SmoothColor animColor;
    protected StraightFade animFade;

    protected virtual void Awake()
    {
      hpBar = GetComponent<HpBar>();
      rigid = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();

      defaultColor = spriteRenderer.color;
      animColor = new SmoothColor(this, defaultColor, value => spriteRenderer.color = value);
      animFade = new StraightFade(this, () => spriteRenderer.color, 1f, value => spriteRenderer.color = value)
      {
        timeout = 1f
      };
      animFade.onEnded += sender => Remove();
      
      Initialize();
    }

    public void Hit(float damage, float playerXPos, float knockBack = 1f)
    {
      if (!canHit) return;
      var knockDir = playerXPos < position.x
        ? new Vector2(1, 2)
        : new Vector2(-1, 2);

      // hit damage
      Managers.Entity.GetEntity<DamageText>(position, x => x.Show(Mathf.RoundToInt(damage)));

      // hp
      status.hp = Mathf.Max(status.hp - damage, 0f);
      hpBar.curHp = status.hp;

      if (status.hp == 0f)
      {
        canHit = false;
        OnDead(knockDir, knockBack * 2f);

        return;
      }

      // knock back
      rigid.AddForce(knockDir * (knockBackPower * knockBack));

      // color
      spriteRenderer.color = hitColor;
      animColor.Start(spriteRenderer.color, Color.white, 3.5f);
    }
    

    protected virtual void OnDead(Vector2 knockDir, float knockBack)
    {
      anim.SetBool(deadAnimParam, true);
      rigid.AddForce(knockDir * (knockBackPower * knockBack));
      animFade.FadeOut(3f);
    }

    protected virtual void Initialize()
    {
      status.hp = status.maxHp;
      hpBar.maxHp = status.maxHp;
      hpBar.curHp = status.hp;
      canHit = true;
      spriteRenderer.color = defaultColor;
    }

    protected virtual void Remove()
    {
      Release();
    }

    public override void OnGet()
    {
      base.OnGet();
      Initialize();
    }
  }
}
