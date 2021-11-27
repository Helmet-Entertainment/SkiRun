using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public static UIManager instance;
    public int theScore;

    [SerializeField] private Image uiProgressFill;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform endLineTransform;
    
    private Vector3 endLinePosition;
    private float fullDistance;

    private void Awake()
    {
        instance = this;
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
        uiProgressFill.fillAmount = value;
    }
    
    public void UpdateScore()
    {
        scoreText.text = "Fans: " + theScore;
    }
    
}
