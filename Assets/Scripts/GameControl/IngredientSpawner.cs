using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IngredientSpawner : MonoBehaviour
{
    private float leftBound;
    private float rightBound;
    private float topBound;
    private float bottomBound;

    [SerializeField] Tilemap _groundTilemap;

    private void Start()
    {
        GetTilemapEdges();
    }

    private void GetTilemapEdges()
    {
        var bounds = _groundTilemap.localBounds;

        leftBound = bounds.center.x - bounds.max.x;
        rightBound = bounds.center.x + bounds.max.x;
        topBound = bounds.center.y + bounds.max.y;
        bottomBound = bounds.center.y - bounds.max.y;
    }

    public void SpawnIngredient(IngredientsScriptableObject ingredient)
    {
        var randomX = Random.Range(leftBound + 0.5f, rightBound - 0.5f);
        var randomY = Random.Range(topBound - 1, bottomBound + 1);

        var spawnLocation = new Vector2(randomX, randomY);

        Instantiate(ingredient.Prefab, spawnLocation, Quaternion.identity);
    }
}
