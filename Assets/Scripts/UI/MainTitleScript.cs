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
    //[SerializeField] private RectTransform choseBonusTitle;

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
                ActiveNextScene();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                Environment.Exit(0);
        }
    }

    private void ActiveNextScene()
    {
        nameInputTitle.SetActive(true);
        nameInput.Select();
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
