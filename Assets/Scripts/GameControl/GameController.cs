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


    [SerializeField] private IngredientsScriptableObject[] _allIngredients;
    [SerializeField] private Witch _witch;
    private void Awake()
    {
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

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void HandleSecondPassed()
    {
        _secondsPassed++;

        if (_secondsPassed >= 10)
        {
            var randomIndex = Random.Range(0, _allIngredients.Length);
            _ingredientSpawner.SpawnIngredient(_allIngredients[randomIndex]);

            _secondsPassed -= 10;
        }
    }

    private void HandleIngredientQueueFull()
    {
        Debug.Log("Game over");
    }

    private void HandleIngredientHandedIn(IngredientsScriptableObject ingredient)
    {
        _score += 1;
    }
}
