using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private GameObject _creditsObject;

    private void OnEnable()
    {
        _volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void OnDisable()
    {
        _volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
    }

    private void Start()
    {
        _volumeSlider.value = AudioManager.Instance.GetVolume();
    }

    private void ChangeVolume(float volume)
    {
        AudioManager.Instance.UpdateVolume(volume);
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
