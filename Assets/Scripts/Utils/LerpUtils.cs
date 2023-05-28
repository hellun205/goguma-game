using UnityEngine;

namespace Utils {
  public static class LerpUtils {
    public static float ColorLerp(float a, float b, float smoothing) {
      var _a = Mathf.Lerp(a, b + (a < b ? 0.4f : -0.4f), Time.deltaTime * smoothing);
      return a < b ? Mathf.Min(_a, b) : Mathf.Max(_a, b);
    }
  }
}