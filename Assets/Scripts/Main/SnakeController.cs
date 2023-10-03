using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SnakeController : MonoBehaviour
{
    // Grid Setup
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private Vector3Int gridPosition;
    private Vector2 _gridPosition;
    private Vector2 _gridMoveDirection;

    // Time - Speed
    private float nextUpdate;
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;

    // Segments - Parts
    private List<Transform> _parts = new List<Transform>();
    [SerializeField] private Transform partPrefab;
    [SerializeField] private int initialSize = 4;

    // Controller - Movement
    private PlayerInput _controls;
    
    private void Start()
    {
        _controls.InGame.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
        ResetState();
    }

    private void Awake()
    {
        // Control enable
        _controls = new PlayerInput();
        speed = 5.0f;
        speedMultiplier = 1.0f;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }


    private void FixedUpdate()
    {
        if (Time.time < nextUpdate) return;

        for (int i = _parts.Count - 1; i > 0; i--)
        {
            _parts[i].transform.position = _parts[i - 1].transform.position;
        }

        GridMovement();

        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
    }

    private void GridMovement()
    {
        if (CanMove(_gridMoveDirection))
        {
            _gridPosition += _gridMoveDirection;
            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(_gridMoveDirection) - 90);
        }
    }

    private void OnMovement(Vector2 direction)
    {

        if (direction != -(_gridMoveDirection))
        {
            _gridMoveDirection = direction;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        gridPosition = groundTileMap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTileMap.HasTile(gridPosition) || wallTileMap.HasTile(gridPosition))
        {
            return false;
        }
        return true;
    }

    private float GetAngleFromVector(Vector2 direction)
    {
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }


    private void Grow()
    {
        Transform segment = Instantiate(partPrefab);
        segment.position = _parts[_parts.Count - 1].position;
        _parts.Add(segment);
    }


    public void ResetState()
    {

        // Initial setup (Snake's start position)
        _gridPosition = transform.position;
        _gridMoveDirection = Vector2.right;

        for (int i = 1; i < _parts.Count; i++)
        {
            Destroy(_parts[i].gameObject);
        }

        // Clear the list but add back this as the head
        _parts.Clear();
        _parts.Add(transform);

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fruit"))
        {
            Grow();
        }
    }

}
