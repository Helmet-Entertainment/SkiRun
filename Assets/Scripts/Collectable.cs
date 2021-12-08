using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public GameObject scoreText;

    private void OnTriggerEnter(Collider other)
    {
        UIManager.instance.theScore += 50;
        Destroy(gameObject);
        
    }
}
