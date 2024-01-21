using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private GameObject _creditsObject;

    private void OnEnable()
    {
        _musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        _effectsVolumeSlider.onValueChanged.AddListener(ChangeEffectsVolume);
    }

    private void OnDisable()
    {
        _musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
        _effectsVolumeSlider.onValueChanged.RemoveListener(ChangeEffectsVolume);
    }

    private void Start()
    {
        _musicVolumeSlider.value = GlobalData.MusicVolume;
        _effectsVolumeSlider.value = GlobalData.EffectsVolume;
    }

    private void ChangeMusicVolume(float volume)
    {
        GlobalData.MusicVolume = volume;
    }
    
    private void ChangeEffectsVolume(float volume)
    {
        GlobalData.EffectsVolume = volume;
    }

    public void NavigateToMainCanvas(GameObject defaultSelect)
    {
        _menuObject.SetActive(true);
        _creditsObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(defaultSelect);
    }

    public void NavigationToCreditsCanvas(GameObject defaultSelect)
    {
        _menuObject.SetActive(false);
        _creditsObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultSelect);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
