using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    public Image flagImage;
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flagImage.color=Color.white;
        }
    }
}
