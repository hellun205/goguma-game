using System;
using UnityEngine;

namespace Audio
{
  [Serializable]
  public class AudioData
  {
    public string name;

    public AudioClip audioClip;

    [Range(-3f, 3f)]
    public float pitch = 1f;

    [Range(0f, 3f)]
    public float delay;

    public bool loop;
  }
}
