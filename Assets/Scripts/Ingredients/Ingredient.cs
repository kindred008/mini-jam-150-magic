using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour, IInteractable
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;

    [SerializeField]
    private IngredientsScriptableObject _ingredientsScriptableObject;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (_spriteRenderer.sprite != null)
        {
            _spriteRenderer.sprite = _ingredientsScriptableObject.Graphic;
        }
    }

    public void Interact()
    {
        Debug.Log($"Interacted with {_ingredientsScriptableObject.Name}");
    }
}
