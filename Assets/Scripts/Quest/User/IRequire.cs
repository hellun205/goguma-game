namespace Quest.User
{
  public interface IRequire
  {
    public void Add();

    public int Max { get; }
    
    public int Current { get; }
  }
}
