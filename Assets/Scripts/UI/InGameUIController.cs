using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private GameObject inGameTitle;
    private float _timeInterval = 0.2f;
    private float _timeSinceLastInterval = 0f;
    private bool flag = true;

    private GameManager manager;
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (inGameTitle.activeSelf)
        {
            _timeSinceLastInterval += Time.deltaTime;

            if (_timeSinceLastInterval >= _timeInterval)
            {
                SetColorRepeat(flag);
                flag = !flag;
                _timeSinceLastInterval = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                manager.StartGame();
                inGameTitle.SetActive(false);
            }
        }
    }
    void SetColorRepeat(bool type)
    {
        TextMeshProUGUI textComponent = inGameTitle.GetComponent<TextMeshProUGUI>();
        switch (type)
        {
            case true:
                textComponent.color = new Color(255, 255, 255);
                return;
            case false:
                textComponent.color = new Color(0, 0, 0);
                return;

        }

    }
}
