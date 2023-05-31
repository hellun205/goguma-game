using System.Collections;
using UnityEngine;

namespace Manager
{
  public class Option : SingleTon<Option>
  {
    public CanvasGroup panel;

    public Coroutine animationCoroutine;
    private bool isActive = false;

    protected override void Awake()
    {
      base.Awake();
      panel.gameObject.SetActive(false);
      panel.alpha = 0f;
    }

    private void Update()
    {
      if (Input.GetKeyDown(Managers.Key.option))
      {
        ToggleActive();
      }
    }

    private void ToggleActive()
    {
      isActive = !isActive;
      if (animationCoroutine is not null)
        StopCoroutine(animationCoroutine);
      animationCoroutine = StartCoroutine(AnimCoroutine(!isActive));
    }

    private IEnumerator AnimCoroutine(bool active)
    {
      var timer = 0f;
      var startAlpha = panel.alpha;
      if (active)
      {
        panel.gameObject.SetActive(true);
        Time.timeScale = 0f;
      }
      while (true)
      {
        yield return new WaitForEndOfFrame();
        timer += Time.unscaledDeltaTime * 4f;
        panel.alpha = Mathf.Lerp(startAlpha, active ? 1f : 0f, timer);

        if (active && panel.alpha >= 1f)
          yield break;
        else if (!active && panel.alpha <= 0f)
        {
          panel.gameObject.SetActive(false);
          Time.timeScale = 1f;
          yield break;
        }
      }
    }
  }
}
