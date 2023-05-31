using UnityEngine;

namespace Manager
{
  public class KeyManager : SingleTon<KeyManager>
  {
    public KeyCode openInventory = KeyCode.I;

    public KeyCode interact1 = KeyCode.Z;

    public KeyCode interact2 = KeyCode.X;

    public KeyCode interact3 = KeyCode.C;

    public KeyCode jump = KeyCode.Space;
  }
}
