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
    private int currentWeapon = 0;

    [SerializeField]
    private int currentAttack = 0;

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
        for (var i = 0; i < attackKeys.Length; i++) {
          if (Input.GetKey(attackKeys[i])) {
            Attack(weapons[currentWeapon], i);
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

    private void Attack(Weapon weapon, int atkType) {
      var attack = weapon.Attacks[atkType];

      movement.canFlip = false;
      currentAttack = atkType;
      anim.SetInteger("weaponType", weapon.type);
      anim.SetInteger("attackType", atkType + 1);
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
      // Attack Hitbox Gizmos
      var attack = weapons[currentWeapon].Attacks[currentAttack];
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(attack.hitBoxPos.position, attack.hitBoxSize);
    }
  }
}