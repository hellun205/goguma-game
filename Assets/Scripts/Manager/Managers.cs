namespace Manager
{
  public class Managers : SingleTon<Managers> , IDontDestroy
  {
    public static AudioManager Audio { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static KeyManager Key { get; private set; }
    public static EntityManager Entity { get; private set; }
    

    protected override void Awake()
    {
      base.Awake();
      Audio = FindObjectOfType<AudioManager>();
      Inventory = FindObjectOfType<InventoryManager>();
      Key = FindObjectOfType<KeyManager>();
      Entity = FindObjectOfType<EntityManager>();
    }
  }
}
