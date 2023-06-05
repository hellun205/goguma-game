using Dialogue;
using Entity.Enemy;
using Entity.Item;
using Entity.Npc;
using Entity.Player.Attack;
using Inventory;
using Inventory.QuickSlot;
using Manager;
using Quest.User;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entity.Player
{
  public class PlayerController : Entity
  {
    public static PlayerController Instance { get; private set; }

    // Components
    // private Rigidbody2D rb;
    private Animator anim;

    private PlayerMovement movement;

    // private AudioSource audioSrc;
    private HpBar hpBar;

    // Inspector Settings
    [SerializeField]
    private KeyCode[] attackKeys;

    [SerializeField]
    private KeyCode meetNpcKey = KeyCode.C;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float checkNpcDistance = 3f;

    [SerializeField]
    private float pickupDistance;


    [SerializeField]
    private Sprite avatar;

    public QuickSlotController quickSlotCtrler;
    public SkillPanel skillPanel;

    [Header("Hand")]
    [SerializeField]
    private SpriteRenderer[] hands;

    // Variables
    private float distanceY;
    public Dialogue.Speaker speakerData => new Dialogue.Speaker(entityName, avatar, AvatarPosition.Left);
    public bool isInputCooldown => movement.isInputCooldown;
    public PlayerStatus status;
    public Inventory.Inventory inventory;

    // Attack Vars
    private bool canCooldown = false;
    private float curCoolTime;
    private float curEndTime;
    private float curKeepTime;
    private byte combo;
    private KeyCode skillType;
    private float tempCoolTime;
    private bool cooled;
    private bool isEnd;

    private Vector2 attackHitPos = Vector2.zero;
    private Vector2 attackHitSize = Vector2.zero;

    [SerializeField]
    private SpriteRenderer effect;

    private static readonly int WeaponType = Animator.StringToHash("weaponType");
    private bool isAttacking;
    private const float useCoolTime = 0.4f;
    
    // Quest
    public QuestData questData = new QuestData();

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      else
        Destroy(gameObject);

      DontDestroyOnLoad(gameObject);

      // rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      movement = GetComponent<PlayerMovement>();
      // audioSrc = GetComponent<AudioSource>();
      col = GetComponent<BoxCollider2D>();
      hpBar = GetComponent<HpBar>();

      quickSlotCtrler.onSlotChanged += OnChangedSlot;

      distanceY = col.bounds.extents.y - 0.1f;
      inventory = new Inventory.Inventory(InventoryManager.horizontalCount * 7);
      InventoryManager.Instance.inventory = inventory;
      canDespawn = false;
      quickSlotCtrler.SetIndex(0);

      DisableAllHand();

      skillPanel.SetCooldown(1f, 1f);
      skillPanel.SetActive(0f);
    }

    private void Update()
    {
      DebugKey();

      skillPanel.SetCooldown(curCoolTime);
      skillPanel.SetActive(curEndTime);

      if (canCooldown && curCoolTime > 0) curCoolTime -= Time.deltaTime;

      if (isAttacking && curCoolTime <= 0) isAttacking = false;

      if (curEndTime > 0) curEndTime -= Time.deltaTime;
      else
      {
        if (isEnd)
        {
          curKeepTime = 0;
          combo = 0;
          isEnd = false;
        }

        movement.canFlip = true;
      }

      if (curKeepTime > 0)
        curKeepTime -= Time.deltaTime;
      else
      {
        anim.SetBool("isAttack", false);

        if (!cooled)
        {
          cooled = true;
          curCoolTime = tempCoolTime;
        }
      }

      if (movement.isInputCooldown ||
          Managers.Window.IsActive ||
          DialogueController.Instance.isEnabled)
        return;

      TryInteract();
      CheckNpc();
      CheckItems();

      if (!Input.anyKeyDown)
        return;

      int slotIdx = Input.inputString switch
      {
        "1" => 0,
        "2" => 1,
        "3" => 2,
        "4" => 3,
        "5" => 4,
        "6" => 5,
        "7" => 6,
        "8" => 7,
        "9" => 8,
        _   => -1
      };

      if (slotIdx != -1)
      {
        quickSlotCtrler.SetIndex((byte) slotIdx);
        Managers.Audio.PlaySFX("click");
      }
    }

    private void DebugKey()
    {
      if (Input.GetKeyDown(KeyCode.F6))
        Managers.Entity.Get<EEDust>(new Vector2(position.x * movement.direction + 1f, position.y + 0.2f));
      else if (Input.GetKeyDown(KeyCode.F7))
        Managers.Entity.Get<Item.EItem>(new Vector2(position.x * movement.direction + 1f, position.y + 0.2f),
          x => x.Init("appleBuff"));
    }

    private void OnChangedSlot(byte slotIdx)
    {
      if (!isAttacking && quickSlotCtrler.previousIndex != slotIdx)
      {
        const float changeCoolTime = 0.2f;
        curCoolTime = changeCoolTime;
        skillPanel.SetCooldown(changeCoolTime, changeCoolTime);
      }

      var item = quickSlotCtrler.GetItem(slotIdx);
      DisableAllHand();
      skillPanel.z.img.sprite = skillPanel.noneSprite;
      skillPanel.x.img.sprite = skillPanel.noneSprite;
      anim.SetInteger(WeaponType, 0);
      canCooldown = item is not null;

      if (item is null)
        return;

      SpriteRenderer hand;
      switch (item)
      {
        case WeaponItem weapon:
        {
          hand = hands[(int) weapon.weaponType];
          hand.sprite = weapon.weaponSprite;
          anim.SetInteger(WeaponType, (int) weapon.weaponType);
          skillPanel.z.img.sprite = weapon.skill.zSprite;
          skillPanel.x.img.sprite = weapon.skill.xSprite;
          break;
        }

        case UseableItem useable:
        {
          hand = hands[0];
          hand.sprite = item.sprite;
          skillPanel.z.img.sprite = item.sprite8x;
          skillPanel.x.img.sprite = item.sprite8x;
          break;
        }

        default:
        {
          hand = hands[0];
          hand.sprite = item.sprite;
          break;
        }
      }

      hand.color = item.spriteColor;
      effect.color = item.effectColor;
      hand.gameObject.SetActive(true);
    }

    private void DisableAllHand()
    {
      foreach (var hand in hands)
        hand.gameObject.SetActive(false);
    }

    private void Attack(Skill skill, KeyCode key)
    {
      var comboSkill = skill.GetComboSkill(key);

      if (curCoolTime > 0 || curEndTime > 0)
        return;

      cooled = false;

      if (skillType == key && curKeepTime > 0)
      {
        combo++;

        if (combo < comboSkill.skills.Length)
        {
          var curSkill = comboSkill.skills[combo];
          StartAttack(skill.damage, curSkill, comboSkill.keepComboTime, comboSkill.coolTime);
        }

        if (combo + 1 >= comboSkill.skills.Length)
          isEnd = true;
      }
      else
      {
        skillType = key;
        StartAttack(skill.damage, comboSkill.skills[0], comboSkill.keepComboTime, comboSkill.coolTime);
        combo = 0;
      }
    }

    private void StartAttack(float weaponDmg, ComboSkill skill, float keepComboTime, float coolTime)
    {
      isAttacking = true;
      curKeepTime = keepComboTime;
      tempCoolTime = coolTime;
      curEndTime = skill.endTime;
      skillPanel.SetActive(skill.endTime, skill.endTime);
      skillPanel.SetCooldown(0f, coolTime);

      // Debug.Log(skill.animParameter);
      movement.canFlip = false;
      anim.SetInteger("attackType", skill.animParameter);
      anim.SetBool("isAttack", true);
      // anim.SetTrigger("attack");
      Managers.Audio.PlaySFX(skill.audio);

      attackHitPos = skill.hitBoxPos;
      attackHitSize = skill.hitBoxSize;
      attackHitPos.x *= (int) movement.currentDirection;

      var colliders = Physics2D.OverlapBoxAll(position + attackHitPos, attackHitSize, 0);
      foreach (var hitCol in colliders)
      {
        if (hitCol.CompareTag("Enemy"))
        {
          var enemy = hitCol.GetComponent<EnemyController>();
          enemy.Hit(weaponDmg * skill.damagePercent, position.x);
        }
      }
    }

    private void OnDrawGizmos()
    {
      Gizmos.DrawWireCube(transform.position + (Vector3) attackHitPos, attackHitSize);
    }

    private void TryInteract()
    {
      foreach (var key in attackKeys)
      {
        if (Input.GetKeyDown(key))
        {
          var item = quickSlotCtrler.GetItem();

          switch (item)
          {
            case null:
              return;

            case WeaponItem weapon:
            {
              Attack(weapon.skill, key);
              break;
            }

            case UseableItem useable:
            {
              if (curCoolTime <= 0)
              {
                useable.OnQuickClick();
                curCoolTime = useCoolTime;
                skillPanel.SetCooldown(useCoolTime, useCoolTime);
              }

              break;
            }
          }
        }
      }
    }

    private void Start()
    {
      GetComponent<NameTag>().OnGetEntityEntity(this);
      hpBar.OnGetEntityEntity(this);
      hpBar.maxHp = status.maxHp;
      hpBar.curHp = status.hp;

      // Entity.SummonNpc(new Vector2(-4.3f, -2.2f), "TallCarrot");

      inventory.GainItem(Managers.Item.GetObject("iron_sword"));
      inventory.GainItem(Managers.Item.GetObject("sword"));
      SceneManager.LoadScene("Scenes/Maps/Test");
    }

    private void CheckNpc()
    {
      if (!Input.GetKeyDown(meetNpcKey))
        return;

      var pos = transform.position;
      var hit = Physics2D.Raycast(new Vector2(pos.x, pos.y - distanceY),
        movement.dirVector, checkNpcDistance, layerMask);

      if (hit && hit.transform.CompareTag("Npc"))
      {
        var npc = hit.transform.GetComponent<ENpc>();

        npc.Meet();
      }
    }

    private void CheckItems()
    {
      var pos = transform.position;
      var hit = Physics2D.Raycast(new Vector2(pos.x, pos.y - distanceY), movement.dirVector, pickupDistance, layerMask);

      if (hit && hit.transform.CompareTag("Item"))
      {
        var item = hit.transform.GetComponent<Item.EItem>();

        if (!item.isPickingUp && !item.isThrowing)
          item.PickUp(transform, OnPickUpItem);
      }
    }

    public void EnableInputCooldown() => movement.EnableInputCooldown();

    private void OnPickUpItem((Item.BaseItem item, byte count) data)
    {
      // Debug.Log($"get: {data.item._name}, count: {data.count}");
      Managers.Audio.PlaySFX("pickup_item");
      var left = inventory.GainItem(data.item, data.count);
      InventoryManager.Instance.Refresh();

      if (left > 0)
        ThrowItem(data.item, left);
    }

    public void ThrowItem(Item.BaseItem item, ushort count) =>
      base.ThrowItem(item, count, (sbyte) (movement.currentDirection == Direction.Left ? -1 : 1));
  }
}
