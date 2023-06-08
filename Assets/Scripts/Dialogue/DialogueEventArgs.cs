using Manager;

namespace Dialogue
{
  public struct DialogueEventArgs
  {
    public void Close() => Managers.Dialogue.Close();
    
    public bool button;
  }
}