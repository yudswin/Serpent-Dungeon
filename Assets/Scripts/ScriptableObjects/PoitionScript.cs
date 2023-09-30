using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoitionScript : MonoBehaviour
{
    [Header("Bounds")]
    private Vector2 boundMax = new Vector2(1, 0);
    private Vector2 boundMin = new Vector2(15, 15);
    private float speed = 5.0f;

    public GameObject spotPoition;
    private Vector2 _targetPosition;
    private GameManager manager;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameManager>();
    }

    private void Update()
    {
        _targetPosition = spotPoition.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, spotPoition.transform.position, speed * Time.deltaTime);

        if (_targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (_targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (Vector2.Distance(transform.position, spotPoition.transform.position) < 0.2f)
        {
            spotPoition.transform.position = new Vector2(Random.Range(boundMin.x, boundMax.x), Random.Range(boundMin.y, boundMax.y));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.AddScore(Random.Range(10, 50));
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Destroy(spotPoition);
    }
}
