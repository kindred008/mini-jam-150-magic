using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour, IInteractable
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;

    [SerializeField]
    private IngredientsScriptableObject _ingredientsScriptableObject;

    public IngredientsScriptableObject IngredientsScriptableObject
    {
        get => _ingredientsScriptableObject;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (_ingredientsScriptableObject.Graphic != null)
        {
            _spriteRenderer.sprite = _ingredientsScriptableObject.Graphic;
        }
    }

    public void ChangeIngredient(IngredientsScriptableObject ingredient)
    {
        _ingredientsScriptableObject = ingredient;
        ChangeSprite();
    }

    public void Interact()
    {
        Debug.Log($"Interacted with {_ingredientsScriptableObject.Name}");
    }
}
