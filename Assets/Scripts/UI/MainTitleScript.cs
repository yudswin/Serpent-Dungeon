using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainTitleScript : MonoBehaviour
{
    [SerializeField] private GameObject mainTitle;

    [SerializeField] private GameObject nameInputTitle;
    [SerializeField] private TMP_InputField nameInput;

    [SerializeField] private GameObject chooseBonusTitle;
    [SerializeField] private GameObject bonusDescription;

    private float _timeInterval = 0.2f;
    private float _timeSinceLastInterval = 0f;
    private bool flag;
    public bool flagGO;


    private void Awake()
    {
        flag = true;
    }

    void Update()
    {
        // Main title
        if (mainTitle.activeSelf)
        {
            _timeSinceLastInterval += Time.deltaTime;

            if (_timeSinceLastInterval >= _timeInterval)
            {
                SetColorRepeat(flag);
                flag = !flag;
                _timeSinceLastInterval = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                mainTitle.SetActive(false);
                ActiveNameInputTitle();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                Environment.Exit(0);
        }

        //Name Input
        if(nameInputTitle.activeSelf)
        {
            if(GameManager.playerName != null)
            {
                nameInputTitle.SetActive(false);
                ActiveBonusTitle();
            }
        }

    }

    private void ActiveNameInputTitle()
    {
        nameInputTitle.SetActive(true);
        nameInput.Select();
    }

    private void ActiveBonusTitle()
    {
        chooseBonusTitle.SetActive(true);
        bonusDescription.SetActive(true);
    }

    void SetColorRepeat(bool type)
    {
        TextMeshProUGUI textComponent = mainTitle.GetComponent<TextMeshProUGUI>();
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
