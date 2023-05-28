namespace Entity.Item
{
  public enum ItemType
  {
    Normal,
    Weapon,
    Wearable,
    Useable,
    Money,
  }

  public static class ItemTypeExtensions
  {
    public static string GetString(this ItemType type) => type switch
    {
      ItemType.Money => "화폐",
      ItemType.Normal => "일반",
      ItemType.Useable => "사용",
      ItemType.Weapon => "무기",
      ItemType.Wearable => "장비",
      _ => type.ToString()
    };
  }
}
