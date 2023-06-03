using Window;

namespace Manager
{
  public class Managers : SingleTon<Managers> , IDontDestroy
  {
    public static AudioManager Audio { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static KeyManager Key { get; private set; }
    public static EntityManager Entity { get; private set; }
    public static Option Option { get; private set; }
    public static PrefabManager Prefab { get; private set; }
    public static WindowManager Window { get; private set; }
    

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
    }
  }
}
