using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
  public class SceneLoader : MonoBehaviour
  {
    public GameObject disableObj;

    public Transform animateObj;

    public Transform afterAnimatePosition;

    public Image fadeOutImg;

    private AsyncOperation loadScene;
    private bool canLoad;

    public void StartButton()
    {
      disableObj.SetActive(false);
      fadeOutImg.raycastTarget = true;
      StartCoroutine(Animation());
      StartCoroutine(LoadScene());
      StartCoroutine(Timer(3f, () =>
      {
        canLoad = true;
      }));
    }

    public void ExitButton()
    {
      Application.Quit();
    }

    private IEnumerator Animation()
    {
      while (true)
      {
        animateObj.position = 
          Vector3.Lerp(animateObj.position, afterAnimatePosition.position, Time.deltaTime * 0.5f);
        
        var color = fadeOutImg.color;
        if (color.a < 1f)
          color.a += Time.deltaTime;
        else yield break;
        fadeOutImg.color = color;
        
        yield return new WaitForEndOfFrame();
      }
    }

    private IEnumerator Timer(float second, Action callback)
    {
      yield return new WaitForSeconds(second);
      callback.Invoke();
    }

    private IEnumerator LoadScene()
    {
      loadScene = SceneManager.LoadSceneAsync("LoadGame");
      loadScene.allowSceneActivation = false;
      while (!loadScene.isDone)
      {
        yield return null;
        if (canLoad && loadScene.progress >= 0.9f)
        {
          loadScene.allowSceneActivation = true;
          yield break;
        }
      }
    }
  }
}
