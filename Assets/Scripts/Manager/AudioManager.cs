using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;

namespace Manager
{
  public class AudioManager : SingleTon<AudioManager>
  {
    private GameObject _sfxGO;

    private GameObject _bgmGO;
    
    private List<AudioSource> _sfxPlayer = new List<AudioSource>();

    private AudioSource _bgmPlayer;
    
    private float _bgmVol = 1f;
    private float _sfxVol = 1f;
    public float BGMVolume
    {
      get => _bgmVol;
      set
      {
        _bgmVol = value;
        _bgmPlayer.volume = value;
      }
    }

    public float SFXVolume
    {
      get => _sfxVol;
      set
      {
        _sfxVol = value;
        _sfxPlayer.ForEach(src => src.volume = value);
      }
    }

    public AudioData[] bgmDatas;

    public AudioData[] sfxDatas;

    protected override void Awake()
    {
      base.Awake();
      if (_bgmGO is null)
        _bgmGO = GameObject.Find("@BGM");
      if (_sfxGO is null)
        _sfxGO = GameObject.Find("@SFX");

      var _component = _bgmGO.GetComponent<AudioSource>();
      if (_component == null)
        _bgmPlayer = _bgmGO.AddComponent<AudioSource>();
      else
        _bgmPlayer = _component;
    }

    public void PlayBGM(string bgmName)
    {
      var sound = bgmDatas.SingleOrDefault(data => data.name == bgmName);
      if (sound is not null)
        PlaySound(_bgmPlayer, sound);
      else
        Debug.LogError($"Can't find BGM audio data: {bgmName}.");
    }
    
    public void PlaySFX(string sfxName)
    {
      var sound = sfxDatas.SingleOrDefault(data => data.name == sfxName);
      if (sound is not null)
      {
        if (_sfxPlayer.Count == 0 || _sfxPlayer.Count(source => !source.isPlaying) == 0)
          _sfxPlayer.Add(_sfxGO.AddComponent<AudioSource>());

        var player = _sfxPlayer.First(source => !source.isPlaying);
        player.volume = _sfxVol;
        PlaySound(player, sound);
      }
      else
        Debug.LogError($"Can't find SFX audio data: {sfxName}.");
    }

    private static void PlaySound(AudioSource source, AudioData audioData, float delay = 0f)
    {
      source.clip = audioData.audioClip;
      source.pitch = audioData.pitch;
      source.loop = audioData.loop;
      source.PlayDelayed(delay + audioData.delay);
    }
  }
}