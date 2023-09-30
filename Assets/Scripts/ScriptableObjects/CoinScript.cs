using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [Header("Bounds")]
    private Vector2 boundMax = new Vector2(1, 0);
    private Vector2 boundMin = new Vector2(15, 15);
    private float speed = 10.0f;

    public GameObject spotCoin;
    private Vector2 _targetPosition;
    private GameManager manager;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameManager>();
    }

    private void Update()
    {
        _targetPosition = spotCoin.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, spotCoin.transform.position, speed * Time.deltaTime);

        if (_targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
        }
        else if (_targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }   

        if (Vector2.Distance(transform.position, spotCoin.transform.position) < 0.2f)
        {
            spotCoin.transform.position = new Vector2(Random.Range(boundMin.x, boundMax.x), Random.Range(boundMin.y, boundMax.y));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.AddScore(1000);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Destroy(spotCoin);
    }
}
