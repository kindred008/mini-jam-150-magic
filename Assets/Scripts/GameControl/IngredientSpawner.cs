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
    [SerializeField] Tilemap _colliderTilemap;

    [SerializeField] GameObject _ingredientPrefab;

    private void Awake()
    {
        GetTilemapEdges();
    }

    private void GetTilemapEdges()
    {
        var bounds = _groundTilemap.localBounds;

        leftBound = bounds.center.x - bounds.max.x;
        rightBound = bounds.center.x + bounds.max.x;
        topBound = bounds.center.y + bounds.max.y + 1; // This +1 is a dumb fix because the game isn't a complete square
        bottomBound = bounds.center.y - bounds.max.y;
    }

    public void SpawnIngredient(IngredientsScriptableObject ingredient)
    {
        var randomX = Random.Range(leftBound, rightBound);
        var randomY = Random.Range(topBound, bottomBound);

        var spawnLocation = new Vector2(randomX, randomY);
        var spawnCell = _colliderTilemap.WorldToCell(spawnLocation);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnLocation, new Vector2(0.2f, 0.2f), 0f);

        //if (_colliderTilemap.HasTile(spawnCell) || colliders.Length > 0)
        if (colliders.Length > 0)
        {
            SpawnIngredient(ingredient);
        } else
        {
            var spawnedIngredient = Instantiate(_ingredientPrefab, spawnLocation, Quaternion.identity);
            spawnedIngredient.GetComponent<Ingredient>().ChangeIngredient(ingredient);
        }
    }
}
