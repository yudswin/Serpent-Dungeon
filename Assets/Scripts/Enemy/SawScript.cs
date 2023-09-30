using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    public float rotationSpeed = 360.0f; // Degrees per second

    private GameObject spot;
    private List<Transform> moveSpots = new List<Transform>();
    private float _speed = 5.0f;
    private float _startWaitTime = 3.0f;
    private float _waitTime;
    private int _currentSpot;

    private void Start()
    {
        SetSpot();
        _waitTime = _startWaitTime;
        _currentSpot = 0;
        transform.position = moveSpots[0].position;
    }

    private void Update()
    {
        // Rotate the saw blade continuously
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        transform.position = Vector2.MoveTowards(transform.position, moveSpots[_currentSpot].transform.position, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpots[_currentSpot].transform.position) < 0.2f)
        {
            if (_waitTime <= 0)
            {
                _currentSpot = ChooseNextSpot(_currentSpot);
                _waitTime = _startWaitTime;
            }
            else
            {
                _waitTime -= Time.deltaTime;
            }
        }
    }

    [ContextMenu("Set Spot Location")]
    private void SetSpot()
    {
        spot = new GameObject("Spot");
        spot.transform.position = new Vector2(0, 0);
        moveSpots.Add(spot.transform);

        spot = new GameObject("Spot");
        spot.transform.position = new Vector2(0, 16);
        moveSpots.Add(spot.transform);

        spot = new GameObject("Spot");
        spot.transform.position = new Vector2(16, 16);
        moveSpots.Add(spot.transform);

        spot = new GameObject("Spot");
        spot.transform.position = new Vector2(16, 0);
        moveSpots.Add(spot.transform);
    }

    private int ChooseNextSpot(int curIndex)
    {
        int nextMove = Random.Range(0, 2);
        if (nextMove == 1)
        {
            curIndex++;
            if (curIndex > moveSpots.Count - 1) curIndex = 0;
        }
        else
        {
            curIndex--;
            if (curIndex < 0) curIndex = moveSpots.Count - 1;
        }
        return curIndex;
    }

    private void OnDestroy()
    {
        foreach (Transform spot in moveSpots)
        {
            Destroy(spot);
        }
    }
}
