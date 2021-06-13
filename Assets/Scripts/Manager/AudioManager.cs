using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制背景音乐和音效
/// </summary>
public class AudioManager : SingletonMono<AudioManager>
{
    private AudioSource audio_music;
    private AudioSource audio_sound;

    protected override void Awake()
    {
        base.Awake();
        audio_music = transform.Find("Music").GetComponent<AudioSource>();
        audio_sound = transform.Find("Sound").GetComponent<AudioSource>();

        //初始化数据
        audio_music.volume = PlayerPrefs.GetFloat(Const.MusicVolume, 0);
        audio_sound.volume = PlayerPrefs.GetFloat(Const.SoundVolume, 0);
    }

    /// <summary>
    /// /改变背景音乐大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeMusicVolume(float value)
    {
        audio_music.volume = value;
    }

    /// <summary>
    /// /改变音效大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundVolume(float value)
    {
        audio_sound.volume = value;
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="path">路径</param>
    public void PlayMusic(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        PlayMusic(audioClip);
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return;
        }
        audio_music.clip = audioClip;
        audio_music.loop = true;
        audio_music.Play();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path">路径</param>
    public void PlaySound(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(path);
        PlaySound(audioClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return;
        }
        //可以同时播放多个音效
        audio_sound.PlayOneShot(audioClip);
    }

}
