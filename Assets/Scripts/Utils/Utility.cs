using System;
using System.Collections.Generic;
using System.Linq;
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
    
    public static T GetTypeProperty<T>(this Type type, string propertyName = "Type")
    {
      var _type = type.GetProperty(propertyName);
      if (_type is not null)
        return (T) _type.GetValue(null);
      
      Debug.LogError("Can't find type property");
      return default;
    }
    
    public static T Random<T>(this IEnumerable<T> enumerable) {
      var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
      return enumerable1[UnityEngine.Random.Range(0, enumerable1.Count())];
    }

    public static bool IsEqual(this Color a, Color b) =>
      Mathf.Approximately(a.a, b.a) && Mathf.Approximately(a.r, b.r) &&
      Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.b);
  }
}
