using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed = 5.0f;

    Vector2 _movement;
    Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _speed *_movement * Time.fixedDeltaTime);
    }

    void OnMovement(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

}
