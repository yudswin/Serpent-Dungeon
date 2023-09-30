using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostScript : MonoBehaviour
{
    [Header("Bounds")]
    private Vector2 boundMax = new Vector2(1, 0);
    private Vector2 boundMin = new Vector2(15, 15);
    private float speed = 5.0f;

    public GameObject spotGhost;
    private Vector2 _targetPosition;

    private void Update()
    {
        _targetPosition = spotGhost.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, spotGhost.transform.position, speed * Time.deltaTime);

        if (_targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (_targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (Vector2.Distance(transform.position, spotGhost.transform.position) < 0.2f)
        {
            spotGhost.transform.position = new Vector2(Random.Range(boundMin.x, boundMax.x), Random.Range(boundMin.y, boundMax.y));
        }

    }

    private void OnDestroy()
    {
        Destroy(spotGhost);
    }
}
