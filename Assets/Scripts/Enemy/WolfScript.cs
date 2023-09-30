using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WolfScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float startWaitTime;
    public GameObject moveSpot;

    private float _waitTime;

    [Header("Bounds")]
    private Vector2 boundMax = new Vector2(1, 0);
    private Vector2 boundMin = new Vector2(15, 15);

    private Vector2 _targetPosition;
    private bool flag;
    private float _timeInterval = 0.2f;
    private float _timeSinceLastInterval = 0f;
    private float _scaler;

    private void Start()
    {
        _waitTime = startWaitTime;
        _scaler = 1.0f;
        moveSpot.transform.position = new Vector2(Random.Range(boundMin.x, boundMax.x), Random.Range(boundMin.y, boundMax.y));
    }

    private void OnDestroy()
    {
            Destroy(moveSpot);
    }

    private void Update()
    {
        _targetPosition = moveSpot.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, moveSpot.transform.position, speed * Time.deltaTime);

        _timeSinceLastInterval += Time.deltaTime;

        if (_timeSinceLastInterval >= _timeInterval)
        {
            UpdateScale(flag);
            flag = !flag;
            _timeSinceLastInterval = 0f;
        }


        if (_targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-_scaler, _scaler, 1f);
        } 
        else if (_targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(_scaler, _scaler, 1f);
        }

        
        if (Vector2.Distance(transform.position, moveSpot.transform.position) < 0.2f)
        {
            if (_waitTime <= 0)
            {
                moveSpot.transform.position = new Vector2(Random.Range(boundMin.x, boundMax.x), Random.Range(boundMin.y, boundMax.y));
                _waitTime = startWaitTime;
            } else
            {
                _waitTime -= Time.deltaTime;
            }
        }
    }

    void UpdateScale(bool type)
    {
        switch (type)
        {
            case true:
                _scaler = 0.8f;
                return;
            case false:
                _scaler = 1.0f;
                return;

        }

    }





}



