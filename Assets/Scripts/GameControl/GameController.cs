using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static UnityEvent GameOver { get; private set; } = new UnityEvent();
    public static UnityEvent<bool> Pause { get; private set; } = new UnityEvent<bool>();
    private bool _gameOver = false;

    private Timer _timer;
    private IngredientSpawner _ingredientSpawner;

    private int _score = 0;
    private int _ingredientsRequested = 0;
    private int _secondsPassed = -1;

    [Header("Config")]
    [SerializeField] private int _secondsForFirstRequest = 5;
    [SerializeField] private int _secondsForEachRequest = 15;
    [SerializeField] private int _itemsTillDifficultyIncrease = 5;
    [SerializeField] private int _secondsQuickerEachDifficulty = 1;

    [Header("References")]
    [SerializeField] private IngredientsScriptableObject[] _allIngredients;
    [SerializeField] private Witch _witch;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _gameOverScoreText;
    [SerializeField] private GameObject gameOverMenuButtonUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _pauseMenuButtonUI;
    [SerializeField] private TextMeshProUGUI _pauseScoreText;

    private void Awake()
    {
        _secondsPassed = _secondsForEachRequest - _secondsForFirstRequest;
        _timer = new Timer(1.0f);
        _ingredientSpawner = GetComponent<IngredientSpawner>();
    }

    private void OnEnable()
    {
        _timer.IntervalPassed.AddListener(HandleSecondPassed);

        _witch.IngredientQueueFull.AddListener(HandleIngredientQueueFull);
        _witch.IngredientHandedIn.AddListener(HandleIngredientHandedIn);

        GameOver.AddListener(HandleGameOver);
        Pause.AddListener(HandlePause);
    }

    private void OnDisable()
    {
        _timer.IntervalPassed.RemoveListener(HandleSecondPassed);

        _witch.IngredientQueueFull.RemoveListener(HandleIngredientQueueFull);
        _witch.IngredientHandedIn.RemoveListener(HandleIngredientHandedIn);

        GameOver.RemoveListener(HandleGameOver);
        Pause.RemoveListener(HandlePause);
    }

    private void Start()
    {
        foreach(IngredientsScriptableObject ingredient in _allIngredients)
        {
            _ingredientSpawner.SpawnIngredient(ingredient);
        }
    }

    private void Update()
    {
        if (_gameOver)
            return;

        _timer.Update(Time.deltaTime);
    }

    private IngredientsScriptableObject RandomIngredient()
    {
        var randomIndex = Random.Range(0, _allIngredients.Length);
        return _allIngredients[randomIndex];
    }

    private void HandleSecondPassed()
    {
        _secondsPassed++;

        /*if (_secondsPassed == 5 || _secondsPassed == 10)
        {
            _ingredientSpawner.SpawnIngredient(RandomIngredient());
        }*/

        if (_secondsPassed >= _secondsForEachRequest)
        {
            _witch.AddToIngredientQueue(RandomIngredient());
            _ingredientsRequested++;
            _secondsPassed -= _secondsForEachRequest;
        }
    }

    private void HandleIngredientQueueFull()
    {
        GameOver.Invoke();
    }

    private void HandleGameOver()
    {
        _gameOver = true;
        _gameOverUI.SetActive(true);
        _gameOverScoreText.text = "Score: " + _score;

        EventSystem.current.SetSelectedGameObject(gameOverMenuButtonUI);
    }

    private void HandlePause(bool paused)
    {
        if (paused)
        {
            _pauseUI.SetActive(true);
            _pauseScoreText.text = "Score: " + _score;
            EventSystem.current.SetSelectedGameObject(_pauseMenuButtonUI);
            Time.timeScale = 0.0f;
        } else
        {
            _pauseUI.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    private void HandleIngredientHandedIn(IngredientsScriptableObject ingredient)
    {
        _score ++;

        if (_score % _itemsTillDifficultyIncrease == 0)
        {
            _secondsForEachRequest -= _secondsQuickerEachDifficulty;
            Debug.Log("Getting harder");
        }

        _ingredientSpawner.SpawnIngredient(ingredient);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
