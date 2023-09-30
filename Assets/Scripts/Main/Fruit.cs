using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public FruitType fruitType;
    
    public string Name { get => fruitType.fruitName; }
    public int Point { set => this.Point = value; get => fruitType.fruitValue; }
    public Sprite Sprite { get => fruitType.fruitSprite; }
    private FruitSpawner _spawner;
    private GameManager _logic;
    private bool isFloating = false;

    [Header("Bounds")]
    private Vector2 boundMax = new Vector2(1, 0);
    private Vector2 boundMin = new Vector2(15, 15);
    private float speed = 5.0f;

    private GameObject spotFruit;

    private void Awake()
    {
        _spawner = FindObjectOfType<FruitSpawner>();
        _logic = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _logic.FruitSelected(fruitType);
            _spawner.DestroyAllFruit();
            _spawner.SpawnAllFruit();
        }
    }

    private void Update()
    {
        if (isFloating)
        {
            transform.position = Vector2.MoveTowards(transform.position, spotFruit.transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, spotFruit.transform.position) < 0.2f)
            {
                spotFruit.transform.position = new Vector2(Random.Range(boundMin.x, boundMax.x), Random.Range(boundMin.y, boundMax.y));
            }
        }
    }

    private void OnDestroy()
    {
        if (isFloating)
        {
            Destroy(spotFruit);
        }
    }

    public void SetFruitFloating(bool boolean)
    {
        if (boolean)
        {
            isFloating = true;
            spotFruit = new GameObject("Fruit Spot");
            spotFruit.transform.position = new Vector2(8, 8);
        }
        else
        {
            isFloating = false;
        }
        
    }
}
