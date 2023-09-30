using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BonusType
{
    SmallSize,
    WallSlide,
    AddTime
}


public class GameManager : MonoBehaviour
{
    public static BonusType starterBonus;
    public static string playerName;
    public static int hiScore = 0;
    public static bool isLose = false;

    private List<GameObject> allObject = new List<GameObject> ();
    private List<int> pointList = new List<int>() {10, 20, 30, 50, 70, 90, 100};
    [SerializeField] private List<FruitType> fruits = new List<FruitType>();
    [SerializeField] private List<TMP_Text> points_Text = new List<TMP_Text>();
    [SerializeField] private List<Image> giftTierOne_Img = new List<Image>();
    [SerializeField] private List<Image> giftTierTwo_Img = new List<Image>();
    [SerializeField] private List<Image> giftTierThree_Img = new List<Image>();
    [SerializeField] private List<Sprite> tierOne = new List<Sprite>();
    [SerializeField] private List<Sprite> tierTwo = new List<Sprite>();
    [SerializeField] private List<Sprite> tierThree = new List<Sprite>();

    [Header("Tier 2 Prefabs")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private GameObject ghostPrefab;

    [Header("Tier 3 Prefabs")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject poitionPrefab;

    private Vector2 minBound = new Vector2(2.0f, 2.0f);
    private Vector2 maxBound = new Vector2(13.0f, 13.0f);
    private Vector2 halfGrid = new Vector2(0.5f, 0.5f);
    private GameObject spot;

    [SerializeField] private GameObject snakePrefab;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    [Header("Losing")]
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject hiScoreText;
    [SerializeField] private GameObject hiscoreNameText;
    [SerializeField] private GameObject hiscoreValueText;
    private TMP_Text _hiScoreName;
    private TMP_Text _hiScoreValue;

    private float currentTime;
    private int initTime = 60;
    private float timeMultiplier = 1.0f;
    private int scoreMultiplier = 1;
    private int _score;
    private bool _setup;

    private SoundManager _sound;
    private Snake _snake;
    private FruitSpawner _fruitSpawn;

    public void StartGame()
    {
        isLose = false;
        Instantiate(snakePrefab, new Vector2(4, 8), Quaternion.identity);
        _snake = FindObjectOfType<Snake>();
        
        _fruitSpawn = GetComponent<FruitSpawner>();
        _fruitSpawn.enabled = true;
        _fruitSpawn.SpawnAllFruit();

        SetStartingBonus();
        SetPoint();
        currentTime = initTime;
        _setup = true;
    }
    private void Awake() 
    {
        scoreText.text = "00000";
        timeText.text = initTime.ToString("D3");
        _sound = GetComponent<SoundManager>();
        _setup = false;
        _hiScoreName = hiscoreNameText.GetComponent<TMP_Text>();
        _hiScoreValue = hiscoreValueText.GetComponent<TMP_Text>();
    }

    private void Update()
    { 
        if (_setup)
        {
            CountDownTimer();
        }

        if (loseText.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("InGame");
            }
        }
    }

    private void CountDownTimer()
    {
        currentTime -= 1 * Time.deltaTime * timeMultiplier;
        int countdown = (int)currentTime;
        timeText.text = countdown.ToString("D3");
        if (currentTime <= 0)
        {
            currentTime = 0;
            Lose();
        }
    }

    public void AddScore(int value)
    {
        _score += value * scoreMultiplier;
        scoreText.text = _score.ToString("D5");
    }

    private void SetStartingBonus()
    {
        switch (starterBonus)
        {
            case BonusType.AddTime:
                initTime += 30;
                break;
            case BonusType.SmallSize:
                _snake.SetInitSize(4);
                break;
            case BonusType.WallSlide:
                // WIP
                break;
        }
    }

    public void FruitSelected(FruitType type)
    {

        for (int i = 0; i < fruits.Count; i++)
        {
            if (type == fruits[i])
            {
                AddScore(fruits[i].fruitValue);
                points_Text[i].text = fruits[i].fruitValue.ToString();
                fruits[i].stacks++;
                if (fruits[i].stacks >= 2)
                {
                    switch (fruits[i].stacks)
                    {
                        case 2: // Random Part Resize 
                            int indexTierOne = SetGiftTierOne();
                            giftTierOne_Img[i].sprite = tierOne[indexTierOne];
                            break;
                        case 3: // Spawn Obstacle
                            int indexTierTwo = SetGiftTierTwo();
                            giftTierTwo_Img[i].sprite = tierTwo[indexTierTwo];
                            break;
                        case 4: // Random Bonus
                            int indexTierThree = SetGiftTierThree();
                            giftTierThree_Img[i].sprite = tierThree[indexTierThree];
                            break;
                    }
                    _sound.PlaySound(Sound.GiftUI);
                } else if (fruits[i].stacks == 1)
                {
                    _sound.PlaySound(Sound.PointUI);
                } 
            }
        }
    }

    private int SetGiftTierOne()
    {
        int numGifts = 3;
        int index = Random.Range(0, numGifts);
        switch (index)
        {
            case 0: // Grow 2 Parts 
                _snake.Grow();
                _snake.Grow();
                break;
            case 1: // Grow 1 Part
                _snake.Grow();
                break;
            case 2: // Shrink 1 Part
                _snake.Shrink();
                break;
        }
        return index;
    }

    private int SetGiftTierTwo()
    {
        int numGift = 8;
        int index = Random.Range(0, numGift);
        switch (index)
        {
            case 5: // Increase Time Speed 
                timeMultiplier += 0.5f;
                break;
            case 6: // Spawn Wolf
                SpawnWolf();
                _sound.PlaySound(Sound.WolfSpawn);
                break;
            case 7: // Spawn 2 Wolfs
                SpawnWolf();
                SpawnWolf();
                _sound.PlaySound(Sound.WolfSpawn);
                break;
            case 0: // Spawn Block
                SpawnBlock();
                _sound.PlaySound(Sound.BlockSpawn);
                break;
            case 1: // Spawn 2 Blocks
                SpawnBlock();
                SpawnBlock();
                _sound.PlaySound(Sound.BlockSpawn);
                break;
            case 2: // Set Floating Fruit
                SetFruitFloating();
                break;
            case 4: // Spawn Saw
                SpawnSaw();
                _sound.PlaySound(Sound.AxeSpawn);
                break;
            case 3: // Spawn 3 Ghost
                SpawnGhost();
                SpawnGhost();
                SpawnGhost();
                _sound.PlaySound(Sound.GhostSpawn);
                break;
        }
        return index;
    }

    private int SetGiftTierThree()
    {
        int numGift = 6;
        int index = Random.Range(0, numGift);
        switch (index)
        {
            case 1: // x2 point
                scoreMultiplier = 2;
                break;
            case 2: // x3 point
                scoreMultiplier = 3;
                break;
            case 0: // Spawn 3 Potions
                SpawnPoition();
                SpawnPoition();
                SpawnPoition();
                break;
            case 3: // Spawn Coin
                SpawnCoin();
                break;
            case 5: // Add 1 Fruit
                IncreaseFruitSpawn(1);
                break;
            case 4: // Add 2 Fruit
                IncreaseFruitSpawn(2);
                break;
        }

        return index;
    }

    [ContextMenu("Increase Fruit")]
    private void IncreaseFruitSpawn(int value)
    {
        _fruitSpawn.initFruitSpawn += value;
    }

    [ContextMenu("Spawn Coin")]
    private void SpawnCoin()
    {
        spot = new GameObject("Coin Spot");
        spot.transform.position = new Vector2((int)Random.Range(5, 11), (int)Random.Range(5, 11));

        GameObject newCoin = Instantiate(coinPrefab);
        CoinScript component = newCoin.GetComponent<CoinScript>();
        component.spotCoin = spot;

        StartCoroutine(DestroyAfterDelay(newCoin));
    }

    [ContextMenu("Spawn Poition")]
    private void SpawnPoition()
    {
        spot = new GameObject("Poition Spot");
        spot.transform.position = new Vector2((int)Random.Range(5, 11), (int)Random.Range(5, 11));

        GameObject newPoition = Instantiate(poitionPrefab, new Vector2(Random.Range(5, 11), Random.Range(5, 11)),Quaternion.identity);
        PoitionScript component = newPoition.GetComponent<PoitionScript>();
        component.spotPoition = spot;
    }

    [ContextMenu("Set Fruit Floating")]
    private void SetFruitFloating()
    {
        _fruitSpawn.floating = true;
        StartCoroutine(ResetFloatingAfterDelay());
    }

    private IEnumerator ResetFloatingAfterDelay()
    {
        //Debug.Log("Reset Floating");
        yield return new WaitForSeconds(3.0f);
        _fruitSpawn.floating = false;
    }


    [ContextMenu("Spawn Block")]
    private void SpawnBlock()
    {
        Instantiate(blockPrefab, GetRandomSpawnLocation() + halfGrid, Quaternion.identity);
    }

    [ContextMenu("Spawn Wolf")]
    private void SpawnWolf()
    {
        spot = new GameObject("Wolf Spot");
        spot.transform.position = new Vector2((int)Random.Range(5, 11), (int)Random.Range(5, 11));
        
        GameObject newWolf = Instantiate(wolfPrefab);
        WolfScript component = newWolf.GetComponent<WolfScript>();
        component.moveSpot = spot;
        allObject.Add(newWolf);
    }

    [ContextMenu("Spawn Ghost")]
    private void SpawnGhost()
    {
        spot = new GameObject("Ghost Spot");
        spot.transform.position = new Vector2((int)Random.Range(5, 11), (int)Random.Range(5, 11));

        GameObject newGhost = Instantiate(ghostPrefab);
        GhostScript component = newGhost.GetComponent<GhostScript>();
        component.spotGhost = spot;

        StartCoroutine(DestroyAfterDelay(newGhost));
    }

    private IEnumerator DestroyAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(15.0f);
        Destroy(obj);   
    }

    [ContextMenu("Spawn Axe")]
    private void SpawnSaw()
    {
        GameObject newAxe = Instantiate(sawPrefab, Vector2.zero,Quaternion.identity);
        allObject.Add(newAxe);
    }

    private Vector2 GetRandomSpawnLocation()
    {
        int x = Mathf.RoundToInt(Random.Range(minBound.x, maxBound.x));
        int y = Mathf.RoundToInt(Random.Range(minBound.y, maxBound.y));

        while (_snake.Occupies(x, y))
        {
            x++;

            if (x > maxBound.x)
            {
                x = Mathf.RoundToInt(minBound.x);
                y++;

                if (y > maxBound.y)
                {
                    y = Mathf.RoundToInt(minBound.y);
                }
            }
        }

        return new Vector2(x, y);
    }


    private void SetPoint()
    {
        foreach (FruitType fruit in fruits)
        {
            int index = Random.Range(0, pointList.Count);
            fruit.fruitValue = pointList[index];
            pointList.RemoveAt(index);
            fruit.stacks = 0;
        }

    }

    public void Lose()
    {
        Time.timeScale = 0.0f;
        if (_score > hiScore)
        {
            hiScore = _score;
            _hiScoreName.text = playerName;
            _hiScoreValue.text = _score.ToString("D5");
        }
        StartCoroutine(DelayTillEnable(hiScoreText, 1.0f));
        StartCoroutine(DelayTillEnable(hiscoreNameText, 2.0f));
        StartCoroutine(DelayTillEnable(hiscoreValueText, 2.0f));
        StartCoroutine(DelayTillEnable(loseText, 3.0f));

        _fruitSpawn.DestroyAllFruit();

        foreach (GameObject obj in allObject)
        {
            Destroy(obj);
        }

        Time.timeScale = 1.0f;
        isLose = true;
    }

    private IEnumerator DelayTillEnable(GameObject obj, float value)
    {
        yield return new WaitForSeconds(value);
        obj.SetActive(true);
    }
}
