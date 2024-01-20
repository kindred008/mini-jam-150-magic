using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Timer _timer;
    private IngredientSpawner _ingredientSpawner;

    [SerializeField] private IngredientsScriptableObject[] _allIngredients;

    private void Awake()
    {
        _timer = new Timer(1.0f);
        _ingredientSpawner = GetComponent<IngredientSpawner>();
    }

    private void OnEnable()
    {
        _timer.IntervalPassed.AddListener(HandleSecondPassed);
    }

    private void OnDisable()
    {
        _timer.IntervalPassed.RemoveListener(HandleSecondPassed);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void HandleSecondPassed()
    {
        
    }
}
