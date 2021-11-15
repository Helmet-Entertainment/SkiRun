using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int scorePenalty = 200;
    public float slowDownSpeed = 1f;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerMovement.HitObstacle(slowDownSpeed, scorePenalty);
        this.enabled = false;
    }
}
