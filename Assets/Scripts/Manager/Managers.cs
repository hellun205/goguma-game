namespace Manager
{
  public class Managers : SingleTon<Managers> , IDontDestroy
  {
    public static AudioManager Audio { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      Audio = FindObjectOfType<AudioManager>();
    }
  }
}
