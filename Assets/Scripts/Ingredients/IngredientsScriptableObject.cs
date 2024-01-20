using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Game/Ingredient")]
public class IngredientsScriptableObject : GameScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private Sprite _graphic;

    [SerializeField]
    private GameObject _prefab;

    public string Name
    {
        get => _name;
    }

    public Sprite Graphic
    {
        get => _graphic;
    }

    public GameObject Prefab
    {
        get => _prefab;
    }
}
