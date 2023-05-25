using UnityEngine;

namespace Utils {
  public static class DebugUtils {
    public static void DrawBox(Vector2 startPos, Vector2 size, Color color, float delay = 0) {
      var rect = new Rect(startPos.x, startPos.y, size.x, size.y);
      Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y ),color,delay);
      Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x , rect.y + rect.height), color,delay);
      Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x + rect.width, rect.y), color,delay);
      Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x, rect.y + rect.height), color,delay);
    }
  }
}