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
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + theScore;
    }
}
