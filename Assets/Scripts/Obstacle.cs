using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float slowDownSpeed = 1f;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerMovement.HitObstacle(slowDownSpeed);
        this.enabled = false;
    }
}
