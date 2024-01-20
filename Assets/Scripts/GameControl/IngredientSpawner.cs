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
        topBound = bounds.center.y + bounds.max.y;
        bottomBound = bounds.center.y - bounds.max.y;
    }

    public void SpawnIngredient(IngredientsScriptableObject ingredient)
    {
        var randomX = Random.Range(leftBound + 0.5f, rightBound - 0.5f);
        var randomY = Random.Range(topBound - 1, bottomBound + 1);

        var spawnLocation = new Vector2(randomX, randomY);
        var spawnCell = _colliderTilemap.WorldToCell(spawnLocation);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnLocation, new Vector2(0.2f, 0.2f), 0f);

        if (_colliderTilemap.HasTile(spawnCell) || colliders.Length > 0)
        {
            SpawnIngredient(ingredient);
        } else
        {
            var spawnedIngredient = Instantiate(_ingredientPrefab, spawnLocation, Quaternion.identity);
            spawnedIngredient.GetComponent<Ingredient>().ChangeIngredient(ingredient);
        }
    }
}
