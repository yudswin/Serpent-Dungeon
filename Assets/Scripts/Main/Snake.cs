using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int _input;
    private Vector2Int direction;
    [SerializeField] private Transform partPrefab;
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private float speedMultiplier = 1f;
    private float _nextUpdate;
    private int initialSize = 6;
    private SoundManager _sound;
    private GameManager _manager;
    private Collider2D _collider;

    private List<Transform> _parts = new List<Transform>();

    public bool wallSlide = false;
    public bool moveThroughWalls = false;

    private void Start()
    {
        
        _sound = GameObject.FindGameObjectWithTag("Logic").GetComponent<SoundManager>();
        _collider = GetComponent<Collider2D>();
        _manager = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameManager>();
        ResetState();
    }

    private void Update()
    {
        // Only allow turning up or down while moving in the x-axis
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _input = Vector2Int.up;
                _sound.PlaySound(Sound.SnakeTurn);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _input = Vector2Int.down;
                _sound.PlaySound(Sound.SnakeTurn);
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _input = Vector2Int.right;
                _sound.PlaySound(Sound.SnakeTurn);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _input = Vector2Int.left;
                _sound.PlaySound(Sound.SnakeTurn);
            }
        }
    }

    public void SetInitSize(int value)
    {
        this.initialSize = value; 
    }

    private void FixedUpdate()
    {
        if (!GameManager.isLose)
        {
            if (Time.time < _nextUpdate)
            {
                return;
            }

            if (_input != Vector2Int.zero)
            {
                this.direction = _input;
            }

            for (int i = _parts.Count - 1; i > 0; i--)
            {
                _parts[i].position = _parts[i - 1].position;
                _parts[i].eulerAngles = _parts[i - 1].eulerAngles;
            }

            int x = Mathf.RoundToInt(this.transform.position.x) + this.direction.x;
            int y = Mathf.RoundToInt(this.transform.position.y) + this.direction.y;
            this.transform.position = new Vector2(x, y);
            this.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(this.direction) - 90);

            _nextUpdate = Time.time + (1f / (speed * speedMultiplier));
        } else
        {
            _nextUpdate = -Time.time;
        }
    }

    private float GetAngleFromVector(Vector2 direction)
    {
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public void Grow()
    {
        Transform segment = Instantiate(partPrefab);
        segment.position = _parts[_parts.Count - 1].position;
        _parts.Add(segment);
    }

    public void Shrink()
    {
        if (_parts.Count > initialSize)
        {
            Transform lastSegment = _parts[_parts.Count - 1];
            _parts.RemoveAt(_parts.Count - 1);
            Destroy(lastSegment.gameObject);
        }
    }

    public void ResetState()
    {

        _collider.enabled = false;
        StartCoroutine(EnableColliderAfterDelay());
        _input = Vector2Int.zero;
        this.direction = Vector2Int.right;
        transform.position = new Vector3(4.0f, 8.0f, 0);

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

    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in _parts)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y)
            {
                return true;
            }
        }

        return false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            _sound.PlaySound(Sound.EatFruit);
        } 
        else if (other.CompareTag("Coin"))
        {
            _sound.PlaySound(Sound.EatCoin);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Obstacle") || other.CompareTag("Body"))
        {
            Debug.Log("Hit!");
            _sound.PlaySound(Sound.SnakeDie);
            _manager.Lose();
        }
    }

    private void Traverse(Transform wall)
    {
        Vector3 position = transform.position;

        if (direction.x != 0f)
        {
            position.x = Mathf.RoundToInt(-wall.position.x + direction.x);
        }
        else if (direction.y != 0f)
        {
            position.y = Mathf.RoundToInt(-wall.position.y + direction.y);
        }

        transform.position = position;
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        _collider.enabled = true;
    }

    private void OnDestroy()
    {
        _parts.Clear();
    }

}