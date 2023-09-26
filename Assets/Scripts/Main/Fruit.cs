using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public FruitType fruitType;
    
    public string Name { get => fruitType.fruitName; }
    public int Point { get => fruitType.fruitValue; }
    public Sprite Sprite { get => fruitType.fruitSprite; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
