using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainMenu
{
  public class GogumaSpawner : MonoBehaviour
  {
    public float min;
    public float max;

    public float time = 0.2f;

    public GameObject prefab;
    
    public byte count;

    public byte maxCount = 30;
    
    private void Start()
    {
      StartCoroutine(SpawnCoroutine(time));
    }

    public IEnumerator SpawnCoroutine(float delay)
    {
      while (true)
      {
        var x = Random.Range(min, max);
        var y = transform.position.y;
        var rotate = Quaternion.Euler(0f,0f, Random.Range(0f, 360f));

        Instantiate(prefab, new Vector3(x,y), rotate);
        count++;
        if (count == maxCount)
          yield break;
        yield return new WaitForSeconds(delay);
      }
    }
  }
}
