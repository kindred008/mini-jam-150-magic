using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Witch : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private List<IngredientsScriptableObject> _ingredientList = new List<IngredientsScriptableObject>();
    private int _maxIngredientQueue;

    [SerializeField]
    private Sprite[] _witchEmotions;

    public UnityEvent IngredientQueueFull { get; private set; } = new UnityEvent();
    public UnityEvent<IngredientsScriptableObject> IngredientHandedIn { get; private set; } = new UnityEvent<IngredientsScriptableObject>();

    public List<IngredientsScriptableObject> IngredientList
    {
        get => _ingredientList;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _maxIngredientQueue = _witchEmotions.Length;
    }

    private void Start()
    {
        ChangeEmotion(_ingredientList.Count);
    }

    public void AddToIngredientQueue(IngredientsScriptableObject ingredient)
    {
        if (_ingredientList.Count + 1 > _maxIngredientQueue)
        {
            IngredientQueueFull.Invoke();
        } else
        {
            _ingredientList.Add(ingredient);

            string allIngredients = string.Join(", ", _ingredientList.Select(x => x.name));
            Debug.Log("The witch wants " + allIngredients);

            ChangeEmotion(_ingredientList.Count - 1);
        }
    }

    public bool HandIngredient(IngredientsScriptableObject ingredient)
    {
        if (_ingredientList.Count == 0)
            return false;

        if (_ingredientList.Remove(ingredient))
        {
            IngredientHandedIn.Invoke(ingredient);
            Debug.Log("Handed in " + ingredient.name);
            ChangeEmotion(_ingredientList.Count - 1);

            return true;
        } else
        {
            Debug.Log("Wrong ingredient");
            return false;
        }

        /*if (ingredient == _ingredientList.Peek())
        {
            IngredientHandedIn.Invoke(_ingredientList.Dequeue());
            Debug.Log("Handed in " + ingredient.Name);
            ChangeEmotion(_ingredientList.Count - 1);

            return true;
        } else
        {
            Debug.Log("Wrong ingredient");
            return false;
        }*/
    }

    private void ChangeEmotion(int spriteIndex)
    {
        if (spriteIndex >= 0)
            _spriteRenderer.sprite = _witchEmotions[spriteIndex];
    }
}
