using UnityEngine;

namespace Manager
{
  public class KeyManager : SingleTon<KeyManager>
  {
    [Header("Inventory")]
    public KeyCode openInventory = KeyCode.I;

    [Header("Item")]
    public KeyCode interact1 = KeyCode.Z;

    public KeyCode interact2 = KeyCode.X;

    [Header("Player movement")]

    public KeyCode jump = KeyCode.Space;

    [Header("Dialogue")]
    public KeyCode yes = KeyCode.Z;

    public KeyCode no = KeyCode.X;

    public KeyCode next = KeyCode.C;

    [Header("Other")]
    public KeyCode option = KeyCode.Escape;
  }
}
