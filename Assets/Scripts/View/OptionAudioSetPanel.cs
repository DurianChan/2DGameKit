using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///选项中的调节音量界面
///</summary>
public class OptionAudioSetPanel : ViewBase
{

    #region  字段
    /// <summary>
    /// 调节音乐条
    /// </summary>
    private Slider slider_music;
    /// <summary>
    /// 调节音效条
    /// </summary>
    private Slider slider_sound;

    #endregion

    #region  事件监听

    public void OnMusicValueChange(float f)
    {
        //对音量大小进行保存
        PlayerPrefs.SetFloat(Const.MusicVolume, f);
        //修改音量大小
        slider_music.value = f;
        AudioManager.Instance.ChangeMusicVolume(f);
    }

    public void OnSoundValueChange(float f)
    {
        //对音量大小进行保存
        PlayerPrefs.SetFloat(Const.SoundVolume, f);
        //修改音量大小 
        slider_sound.value = f;
        AudioManager.Instance.ChangeSoundVolume(f);
    }

    #endregion

    #region  Unity回调

    private void Awake()
    {
        //初始化音量条
        slider_music = transform.Find("slider_music").GetComponent<Slider>();
        slider_music.onValueChanged.AddListener(OnMusicValueChange);
        slider_sound = transform.Find("slider_sound").GetComponent<Slider>();
        slider_sound.onValueChanged.AddListener(OnSoundValueChange);
    }


    #endregion

    #region  重写方法

    public override void Show()
    {
        base.Show();
        slider_music.value = PlayerPrefs.GetFloat(Const.MusicVolume, 0);

        slider_sound.value = PlayerPrefs.GetFloat(Const.SoundVolume, 0);

    }

    #endregion

}
