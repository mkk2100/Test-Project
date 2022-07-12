using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer mixer;
    public AudioSource backgroundMusic;
    public AudioClip[] BGMList;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(this.gameObject);
    }

    public void SFXPlay(string sfxName, AudioClip audioClip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = audioClip;
        //audioSource.volume = 0.1f;
        audioSource.Play();
        Destroy(go, audioClip.length);
    }

    public void BackgroundMusicPlay(AudioClip audioClip, bool isLoop)
    {
        backgroundMusic.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        backgroundMusic.clip = audioClip;
        backgroundMusic.loop = isLoop;
        //backgroundMusic.volume = 0.1f;
        backgroundMusic.Play();
    }

    public void MasterVolume(float masterValue)
    {
        mixer.SetFloat("MasterMixer", Mathf.Log10(masterValue) * 20);
    }

    public void SFXVolume(float sfxValue)
    {
        mixer.SetFloat("SFXMixer", Mathf.Log10(sfxValue) * 20);
    }

    public void BackgroundMusicVolume(float musicValue)
    {
        mixer.SetFloat("BGMMixer", Mathf.Log10(musicValue) * 20);
    }
}
