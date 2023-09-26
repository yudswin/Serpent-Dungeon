using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private List<FruitType> fruits;
    [SerializeField] private GameObject fruitPrefab;


    // Time interval between fruit spawns
    public float spawnInterval = 2f; 
    private float timeSinceLastSpawn = 0f;

    private float _spawnRadius;
    private Vector3 _spawnPoint;


    private void Awake()
    {
        _spawnRadius = 6.0f;
        _spawnPoint = new Vector3(7.5f, 7.5f, 0);
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnFruit();
            SpawnFruit();
            SpawnFruit();
            timeSinceLastSpawn = 0f;
        }
    }


    private void SpawnFruit()
    {
        if (fruits.Count == 0)
        {
            Debug.Log("Fruit not set in the inspector.");
        }

        int randomIndex = Random.Range(0, fruits.Count);
        FruitType selectedFruit = fruits[randomIndex];
        GameObject newFruit = Instantiate(fruitPrefab);

        // Set fruit's component
        Fruit fruitComponent = newFruit.AddComponent<Fruit>();
        fruitComponent.fruitType = selectedFruit;

        // Set fruit's sprite
        SpriteRenderer spriteRenderer = newFruit.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = selectedFruit.fruitSprite;
        spriteRenderer.sortingOrder = 1;

        // Set fruit's position
        newFruit.transform.position = GetRandomSpawnPosition();

    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(_spawnPoint.x - _spawnRadius, _spawnPoint.x + _spawnRadius);
        float y = Random.Range(_spawnPoint.y - _spawnRadius, _spawnPoint.y + _spawnRadius);
        return new Vector3(x, y, 0.0f);
    }
}
