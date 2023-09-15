using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Snake : MonoBehaviour
{

    //[Header("Testing")]
    //public Vector3 x;
    //public Vector3 y;

    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private Vector3Int gridPosition;

    private float _gridMoveTimer;
    private float _gridMoveTimerMax;
    private Vector2 _gridPosition;
    private Vector2 _gridMoveDirection;

    //Controller - Movement
    private PlayerInput _controls;
    

    private void Start()
    {
        _controls.InGame.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
    }

    private void Awake()
    {
        //Testing variables
        //x = new Vector3(0, 0, 0);
        //y = new Vector3(1, 1, 1);

        //x = -x;
        //y = -y;


        // Control enable
        _controls = new PlayerInput();

        // Initial setup (Snake's start position)
        _gridPosition = transform.position;
        _gridMoveDirection = Vector2.right;

        // Timer setup (Snake's speed)
        _gridMoveTimerMax = 0.2f;
        _gridMoveTimer = _gridMoveTimerMax;
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
        GridMovement();
    }

    private void GridMovement()
    {
        if (CanMove(_gridMoveDirection))
        {
            _gridMoveTimer += Time.fixedDeltaTime;

            if (_gridMoveTimer >= _gridMoveTimerMax)
            {
                _gridPosition += _gridMoveDirection;
                _gridMoveTimer -= _gridMoveTimerMax;
            }

            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(_gridMoveDirection));
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
}
