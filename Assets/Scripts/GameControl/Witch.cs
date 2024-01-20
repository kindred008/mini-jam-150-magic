using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Witch : MonoBehaviour
{
    private Queue<IngredientsScriptableObject> _ingredientQueue = new Queue<IngredientsScriptableObject>();

    [SerializeField]
    private int _maxIngredientQueue = 5;

    public UnityEvent IngredientQueueFull = new UnityEvent();
    public UnityEvent<IngredientsScriptableObject> IngredientHandedIn = new UnityEvent<IngredientsScriptableObject>();

    public Queue<IngredientsScriptableObject> IngredientQueue
    {
        get => _ingredientQueue;
    }

    public void AddToIngredientQueue(IngredientsScriptableObject ingredient)
    {
        if (_ingredientQueue.Count + 1 > _maxIngredientQueue)
        {
            IngredientQueueFull.Invoke();
        } else
        {
            IngredientQueue.Enqueue(ingredient);
            Debug.Log("The witch wants " + ingredient.name);
        }
    }

    public bool HandIngredient(IngredientsScriptableObject ingredient)
    {
        if (_ingredientQueue.Count == 0)
            return false;

        if (ingredient == _ingredientQueue.Peek())
        {
            IngredientHandedIn.Invoke(_ingredientQueue.Dequeue());
            Debug.Log("Handed in " + ingredient.Name);
            return true;
        } else
        {
            Debug.Log("Wrong ingredient");
            return false;
        }
    }
}
