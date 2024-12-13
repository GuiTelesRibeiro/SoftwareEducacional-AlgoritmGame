using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChageAudio : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider efxSlider;

    private const string MUSIC_PREF = "MusicVolume";
    private const string EFX_PREF = "EFXVolume";

    private void Start()
    {
        GetMusicValue();
        GetEFXValue();
    }

    private void SetMusicValue(float value)
    {
        // Salva o valor atual no PlayerPrefs
        PlayerPrefs.SetFloat(MUSIC_PREF, value);
        PlayerPrefs.Save();
    }

    private void SetEFXValue(float value)
    {
        // Salva o valor atual no PlayerPrefs
        PlayerPrefs.SetFloat(EFX_PREF, value);
        PlayerPrefs.Save();
    }

    private void GetMusicValue()
    {
        // Busca o valor salvo no PlayerPrefs, ou usa um valor padrão se não existir
        float savedValue = PlayerPrefs.GetFloat(MUSIC_PREF, 3f); // Valor padrão é 3
        musicSlider.value = savedValue;

        // Aplica o valor no AudioMixer
        ChangeValueMusic(musicSlider);
    }

    private void GetEFXValue()
    {
        // Busca o valor salvo no PlayerPrefs, ou usa um valor padrão se não existir
        float savedValue = PlayerPrefs.GetFloat(EFX_PREF, 3f); // Valor padrão é 3
        efxSlider.value = savedValue;

        // Aplica o valor no AudioMixer
        ChangeValueEFX(efxSlider);
    }

    public void ChangeValueMusic(Slider slider)
    {
        switch (slider.value)
        {
            case 0:
                audioMixer.SetFloat("Music", -88);
                SetMusicValue(0);
                break;
            case 1:
                audioMixer.SetFloat("Music", -40);
                SetMusicValue(1);
                break;
            case 2:
                audioMixer.SetFloat("Music", -20);
                SetMusicValue(2);
                break;
            case 3:
                audioMixer.SetFloat("Music", -10);
                SetMusicValue(3);
                break;
            case 4:
                audioMixer.SetFloat("Music", 0);
                SetMusicValue(4);
                break;
            case 5:
                audioMixer.SetFloat("Music", 10);
                SetMusicValue(5);
                break;
        }
    }

    public void ChangeValueEFX(Slider slider)
    {
        switch (slider.value)
        {
            case 0:
                audioMixer.SetFloat("EFX", -88);
                SetEFXValue(0);
                break;
            case 1:
                audioMixer.SetFloat("EFX", -40);
                SetEFXValue(1);
                break;
            case 2:
                audioMixer.SetFloat("EFX", -20);
                SetEFXValue(2);
                break;
            case 3:
                audioMixer.SetFloat("EFX", -10);
                SetEFXValue(3);
                break;
            case 4:
                audioMixer.SetFloat("EFX", 0);
                SetEFXValue(4);
                break;
            case 5:
                audioMixer.SetFloat("EFX", 10);
                SetEFXValue(5);
                break;
        }
    }
}
