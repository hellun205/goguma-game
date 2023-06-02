﻿using System;
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

    public static T Random<T>(this IEnumerable<T> enumerable)
    {
      var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
      return enumerable1[UnityEngine.Random.Range(0, enumerable1.Count())];
    }

    public static bool IsEqual(this Color a, Color b) =>
      Mathf.Approximately(a.a, b.a) && Mathf.Approximately(a.r, b.r) &&
      Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.b);

    public static bool IsEqual(this Vector2 a, Vector2 b) =>
      Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);

    public static bool IsEqual(this Vector3 a, Vector3 b) =>
      Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);

    public static Color Setter(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
    {
      Color result;
      
      result.r = r ?? color.r;
      result.g = r ?? color.g;
      result.b = r ?? color.b;
      result.a = r ?? color.a;
      
      return result;
    }
  }
}
