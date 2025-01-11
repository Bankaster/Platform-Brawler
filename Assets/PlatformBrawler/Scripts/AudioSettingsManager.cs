using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager instance;

    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        float masterVolume, musicVolume, sfxVolume;
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        audioMixer.GetFloat("MusicVolume", out musicVolume);
        audioMixer.GetFloat("SFXVolume", out sfxVolume);

        masterSlider.value = Mathf.Pow(10, masterVolume / 20);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}


