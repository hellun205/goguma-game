namespace Quest.User
{
  public interface IRequire
  {
    public void Add();

    public int Max { get; }
    
    public int Current { get; }

    public virtual bool IsComplete => Max <= Current;
  }
}
