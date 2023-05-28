using System;
using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity.Enemy
{
  public class EnemyController : Entity
  {
    public override EntityType type => EntityType.Enemy;


    public EnemyStatus status;

    [SerializeField]
    protected Color hitColor = Color.red;

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

    private Coroutine hitCoroutine;

    private void Awake()
    {
      hpBar = GetComponent<HpBar>();
      rigid = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      Initialize();
    }

    public void Hit(float damage, float playerXPos, float knockBack = 1f)
    {
      if (!canHit) return;
      var knockDir = playerXPos < position.x
        ? new Vector2(1, 2)
        : new Vector2(-1, 2);

      // hit damage
      Entity.SummonHitDamage(position, Mathf.RoundToInt(damage));

      // hp
      status.hp = Mathf.Max(status.hp - damage, 0f);
      hpBar.curHp = status.hp;

      if (status.hp == 0f)
      {
        canHit = false;
        StopHit();
        OnDead(knockDir, knockBack * 2f);

        return;
      }

      // knock back
      rigid.AddForce(knockDir * (knockBackPower * knockBack));

      // color
      StopHit();
      spriteRenderer.color = hitColor;
      hitCoroutine = StartCoroutine(HitCoroutine());
    }

    private void StopHit()
    {
      if (hitCoroutine is not null) StopCoroutine(hitCoroutine);
    }

    protected virtual IEnumerator HitCoroutine() => ChangeColorSmooth(Color.white, 3.5f);

    protected virtual void OnDead(Vector2 knockDir, float knockBack)
    {
      anim.SetBool(deadAnimParam, true);
      rigid.AddForce(knockDir * (knockBackPower * knockBack));
      StartCoroutine(DeadCoroutine());
    }

    protected virtual IEnumerator DeadCoroutine() => ChangeColorSmooth(Color.clear, 3f, Remove);

    protected IEnumerator ChangeColorSmooth(Color toColor, float smoothing = 3f, [CanBeNull] Action callback = null)
    {
      while (true)
      {
        var color = spriteRenderer.color;

        float colorLerp(float a, float b)
        {
          var _a = Mathf.Lerp(a, b + (a < b ? 0.4f : -0.4f), Time.deltaTime * smoothing);
          return a < b ? Mathf.Min(_a, b) : Mathf.Max(_a, b);
        }

        if (!color.Equals(toColor))
        {
          color.r = colorLerp(color.r, toColor.r);
          color.g = colorLerp(color.g, toColor.g);
          color.b = colorLerp(color.b, toColor.b);
          color.a = colorLerp(color.a, toColor.a);
          spriteRenderer.color = color;
          yield return new WaitForNextFrameUnit();
        }
        else
        {
          callback?.Invoke();
          yield break;
        }
      }
    }

    protected virtual void Initialize()
    {
      status.hp = status.maxHp;
      hpBar.maxHp = status.maxHp;
      hpBar.curHp = status.hp;
      canHit = true;
      spriteRenderer.color = Color.white;
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
