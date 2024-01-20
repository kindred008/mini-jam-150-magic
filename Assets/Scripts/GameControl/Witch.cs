using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Witch : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Queue<IngredientsScriptableObject> _ingredientQueue = new Queue<IngredientsScriptableObject>();
    private int _maxIngredientQueue;

    [SerializeField]
    private Sprite[] _witchEmotions;

    public UnityEvent IngredientQueueFull = new UnityEvent();
    public UnityEvent<IngredientsScriptableObject> IngredientHandedIn = new UnityEvent<IngredientsScriptableObject>();

    public Queue<IngredientsScriptableObject> IngredientQueue
    {
        get => _ingredientQueue;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _maxIngredientQueue = _witchEmotions.Length;
    }

    private void Start()
    {
        ChangeEmotion(_ingredientQueue.Count);
    }

    public void AddToIngredientQueue(IngredientsScriptableObject ingredient)
    {
        if (_ingredientQueue.Count + 1 > _maxIngredientQueue)
        {
            IngredientQueueFull.Invoke();
        } else
        {
            _ingredientQueue.Enqueue(ingredient);
            Debug.Log("The witch wants " + ingredient.name);

            ChangeEmotion(_ingredientQueue.Count - 1);
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

    private void ChangeEmotion(int spriteIndex)
    {
        _spriteRenderer.sprite = _witchEmotions[spriteIndex];
    }
}
