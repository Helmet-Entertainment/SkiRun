using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private PlayerTargetFollower playerMovement;
    
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerTargetFollower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerMovement.HitObstacle();
        this.enabled = false;
    }
}
