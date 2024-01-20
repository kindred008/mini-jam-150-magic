using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
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
    }

    private void OnDisable()
    {
        _timer.IntervalPassed.RemoveListener(HandleSecondPassed);

        _witch.IngredientQueueFull.RemoveListener(HandleIngredientQueueFull);
        _witch.IngredientHandedIn.RemoveListener(HandleIngredientHandedIn);
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
        Debug.Log("Game over");
        Debug.Log("Score: " + _score);
    }

    private void HandleIngredientHandedIn(IngredientsScriptableObject ingredient)
    {
        _score += 1;

        if (_score % _itemsTillDifficultyIncrease == 0)
        {
            _secondsForEachRequest -= _secondsQuickerEachDifficulty;
            Debug.Log("Getting harder");
        }

        _ingredientSpawner.SpawnIngredient(ingredient);
    }
}
