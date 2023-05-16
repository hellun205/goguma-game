using System;
using System.Linq;
using Player.Attack;
using UnityEngine;

namespace Player {
  public class PlayerController : MonoBehaviour {
    public static PlayerController Instance { get; private set; }
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerMovement movement;
    private AudioSource audioSrc;

    [SerializeField]
    private KeyCode[] attackKeys;

    [SerializeField]
    private Weapon[] weapons;

    [SerializeField]
    private Weapons currentWeapon = 0;

    [SerializeField]
    private SkillType currentAttack = 0;

    [SerializeField] private bool hasWeapon => anim.GetBool("hasWeapon");

    private float curCoolTime;
    private float curEndTime;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);

      rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      movement = GetComponent<PlayerMovement>();
      audioSrc = GetComponent<AudioSource>();
    }

    private void OnDestroy() {
      if (Instance == this) Instance = null;
    }

    private void Update() {
      TryAttack();
    }

    private void TryAttack() {
      if (!hasWeapon) return;

      if (curCoolTime <= 0) {
        foreach (var key in attackKeys) {
          if (Input.GetKey(key)) {
            Attack(weapons.Get(currentWeapon), key.GetSkill());
            break;
          }
        }
      } else {
        curCoolTime -= Time.deltaTime;
      }

      if (curEndTime <= 0) {
        EndAttack();
      } else {
        curEndTime -= Time.deltaTime;
      }
    }

    private void Attack(Weapon weapon, SkillType atkType) {
      var attack = weapon.Attacks.Get(atkType);

      movement.canFlip = false;
      currentAttack = atkType;
      anim.SetInteger("weaponType", (int)weapon.type);
      anim.SetInteger("attackType", (int)atkType);
      anim.SetBool("isAttack", true);
      anim.SetTrigger("attack");
      curCoolTime = attack.coolTime;
      curEndTime = attack.endTime;

      audioSrc.clip = attack.sound;
      audioSrc.PlayDelayed(attack.soundDelay);

      var colliders = Physics2D.OverlapBoxAll(attack.hitBoxPos.position, attack.hitBoxSize, 0);
      foreach (var col in colliders) {
        // Attack Feature
        Debug.Log(col.tag);
      }
    }

    private void EndAttack() {
      anim.SetBool("isAttack", false);
      movement.canFlip = true;
    }

    private void OnDrawGizmos() {
      // Attack HitBox Gizmos
      var attack = weapons.Get(currentWeapon).Attacks.Get(currentAttack);
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(attack.hitBoxPos.position, attack.hitBoxSize);
    }

    public bool ChangeWeapon(Weapons type) {
      if (!weapons.Select(weapon => weapon.type).Contains(type)) return false;

      currentWeapon = type;
      anim.SetInteger("weaponType", (int)type);
      anim.SetBool("hasWeapon", type != Weapons.None);
      
      return true;
    }
  }
}