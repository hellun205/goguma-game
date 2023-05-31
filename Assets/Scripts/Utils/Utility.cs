using UnityEngine;

namespace Utils
{
  public static class Utility
  {
    private static UnityEngine.Camera Cam => UnityEngine.Camera.main;

    public static Vector3 WorldToScreenSpace(this RectTransform canvas, Vector3 worldPos)
    {
      Vector3 screenPoint = Cam.WorldToScreenPoint(worldPos);
      screenPoint.z = 0;

      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, Cam, out var screenPos))
        return screenPos;

      return screenPoint;
    }
  }
}
