using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum SpawnZone
{
    TOP,
    MID,
    BOT
}

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private List<FruitType> fruitTypes;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private TilemapCollider2D ground;

    public int initFruitSpawn = 2;
    public bool floating;
    private Snake _snake;
    private List<GameObject> _fruitsOnBoard = new List<GameObject>();
    private List<int> _fruitIndex = new List<int>();


    private void OnEnable()
    {
        _snake = FindObjectOfType<Snake>();
        floating = false;
    }

    public void SpawnAllFruit()
    {
        float deleyEach = 1.0f / initFruitSpawn;
        for (int i = 0; i < initFruitSpawn; i++)
        {
            StartCoroutine(SpawnAfterDelay(deleyEach * (i + 1)));
        }
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int enumLength = System.Enum.GetValues(typeof(SpawnZone)).Length;
        int randomIndex = Random.Range(0, enumLength);
        GetRandomSpawnPosition((SpawnZone)randomIndex);
       
    }


    private void SpawnFruit(int x, int y)
    {
        if (fruitTypes.Count == 0)
        {
            Debug.Log("Fruit not set in the inspector.");
        }

        int randomIndex = Random.Range(0, fruitTypes.Count);
        FruitType selectedFruit = fruitTypes[randomIndex];
        GameObject newFruit = Instantiate(fruitPrefab);

            // Set fruit's component
        Fruit fruitComponent = newFruit.AddComponent<Fruit>();
        fruitComponent.fruitType = selectedFruit;
        if (floating) fruitComponent.SetFruitFloating(true);
        else if (!floating) fruitComponent.SetFruitFloating(false);

            // Set fruit's sprite
        SpriteRenderer spriteRenderer = newFruit.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = selectedFruit.fruitSprite;
        spriteRenderer.sortingOrder = 3;

            // Set fruit's position
        newFruit.transform.position = new Vector2(x,y);
        _fruitsOnBoard.Add(newFruit);
        _fruitIndex.Add(randomIndex);
    }

    public void DestroyAllFruit()
    {
        foreach (GameObject fruit in _fruitsOnBoard)
        {
            Destroy(fruit.gameObject);
            _fruitIndex.Clear();
        }
    }

    private void GetRandomSpawnPosition(SpawnZone zone) 
    {
        float boundXMax = 15.5f;
        float boundXMin = 0.5f;
        float boundYMax = 15.5f;
        float boundYMin = 0.5f;

        switch (zone)
        {
            case SpawnZone.TOP:
                boundXMax = 15.5f;
                boundXMin = 11.0f;
                break;
            case SpawnZone.MID:
                boundXMax = 11.0f;
                boundXMin = 5.0f;
                break;
            case SpawnZone.BOT:
                boundXMax = 5.0f;
                boundXMin = 0.5f;
                break;
        }

        int x = Mathf.RoundToInt(Random.Range(boundXMax, boundXMin));
        int y = Mathf.RoundToInt(Random.Range(boundYMax,boundYMin));

        while (_snake.Occupies(x,y))
        {
            x++;

            if (x > boundXMax)
            {
                x = Mathf.RoundToInt(boundXMin);
                y++;

                if (y > boundYMax)
                {
                    y = Mathf.RoundToInt(boundYMin);
                }
            }
        }

        SpawnFruit(x, y);
    }
}
