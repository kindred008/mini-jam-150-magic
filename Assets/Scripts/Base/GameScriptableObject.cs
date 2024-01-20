using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScriptableObject : ScriptableObject
{
    [SerializeField]
    private string _id;

    public string Id
    {
        get => _id;
    }
}
