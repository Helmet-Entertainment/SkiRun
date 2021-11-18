using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public static UIManager instance;
    public int theScore;
   
    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore()
    {
        scoreText.text = "Fans: " + theScore;
    }
}
