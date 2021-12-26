using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public static UIManager instance;
    public int theScore;
    public Image sliderImage;
    public GameObject winPanel, startPanel, losePanel;

    [SerializeField] private Image uiProgressFill;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform endLineTransform;
    
    private Vector3 endLinePosition;
    private float fullDistance;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 0;
        startPanel.SetActive(true);
    }

    private void Start()
    {
        endLinePosition = endLineTransform.position;
        fullDistance = GetDistance();
    }

    private void Update()
    {
        if (playerTransform.position.z <= endLinePosition.z)
        {
            float newDistance = GetDistance();
            float progressValue = Mathf.InverseLerp(fullDistance, 0f, newDistance);
            UpdateProgressFill(progressValue);   
        }
    }

    private float GetDistance()
    {
        return Vector3.Distance(playerTransform.position, endLinePosition);
    }

    private void UpdateProgressFill(float value)
    {
        uiProgressFill.fillAmount = value + 0.01f;
        sliderImage.color = Color.Lerp(Color.red, Color.green, value + 0.01f);
    }
    
    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void WinGame()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(0);
    }

    public void EnableWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void EnableLosePanel()
    {
       losePanel.SetActive(true);
    }
}
