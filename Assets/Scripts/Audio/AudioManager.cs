using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audio {
  [ExecuteInEditMode]
  public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }
    
    public GameObject audioSrcs;

    [Range(0f, 1f)]
    public float volume = 1f;

    public AudioData[] clips;

    private List<AudioSource> srcs;
    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(this);

      srcs = audioSrcs.GetComponents<AudioSource>().ToList();
      SetVolumes(volume);
    }

    public void PlaySound(AudioData audioData, float delay = 0f) {
      var src = srcs.Where(source => !source.isPlaying).ToArray()[0];
      src.clip = audioData.audioClip;
      src.pitch = audioData.pitch;
      src.loop = audioData.loop;
      src.PlayDelayed(delay + audioData.delay);
    }

    public void PlaySound(string clipName, float delay = 0f) =>
      PlaySound(clips.Where(clip => clip.name == clipName).Single(), delay);

    public void SetVolumes(float value) {
      if (srcs!= null && srcs.Count > 0)
      foreach (var src in srcs) {
        src.volume = value;
      }
    }

    private void OnValidate() {
      SetVolumes(volume);
    }

    public static void Play(AudioData audio, float delay = 0f) => Instance.PlaySound(audio, delay);

    public static void Play(string clipName, float delay = 0f) => Instance.PlaySound(clipName, delay);

    public static void SetVolume(float volume) => Instance.SetVolumes(volume);
  }
}