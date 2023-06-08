using Dialogue;
using Entity;
using Entity.Item;
using Entity.Npc;
using Entity.Player;
using Inventory;
using Quest;
using Window;

namespace Manager
{
  public sealed class Managers : SingleTon<Managers> , IDontDestroy
  {
    public static AudioManager Audio { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static KeyManager Key { get; private set; }
    public static EntityManager Entity { get; private set; }
    public static Option Option { get; private set; }
    public static PrefabManager Prefab { get; private set; }
    public static WindowManager Window { get; private set; }
    public static ItemManager Item { get; private set; }
    public static NpcManager Npc { get; private set; }
    public static QuestManager Quest { get; private set; }
    public static PlayerController Player { get; private set; }
    public static DialogueController Dialogue { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      Audio = FindObjectOfType<AudioManager>();
      Inventory = FindObjectOfType<InventoryManager>();
      Key = FindObjectOfType<KeyManager>();
      Entity = FindObjectOfType<EntityManager>();
      Option = FindObjectOfType<Option>();
      Prefab = FindObjectOfType<PrefabManager>();
      Window = FindObjectOfType<WindowManager>();
      Item = FindObjectOfType<ItemManager>();
      Npc = FindObjectOfType<NpcManager>();
      Quest = FindObjectOfType<QuestManager>();
      Player = FindObjectOfType<PlayerController>();
      Dialogue = FindObjectOfType<DialogueController>();
    }
  }
}
