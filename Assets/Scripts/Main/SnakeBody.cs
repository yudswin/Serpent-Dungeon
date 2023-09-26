using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private int _waitPreviousParts;
    private Vector2 _nextBodyPosition;

    public void SetTargetPosition(Vector3 position)
    {
        if (_waitPreviousParts > 0)
        {
            _waitPreviousParts--;
            return;
        }

        _nextBodyPosition = position;
    }

    public void WaitHeadUpdateCycles(int count)
    {
        _waitPreviousParts = count;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, _nextBodyPosition, Time.deltaTime);
    }
}
