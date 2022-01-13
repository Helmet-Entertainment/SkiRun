using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private float distanceToPlayer;
    private float startX, startY, startZ;
    private float elapsedTime = 0;
    private bool finger;
    private Vector3 oldPos;
    private void Start()
    {
        startX = transform.localPosition.x;
        startY = transform.localPosition.y;
        startZ = transform.localPosition.z;
        oldPos = new Vector3(startX, startY, startZ);
    }

    private void Update()
    {
        var z = Mathf.Sqrt(Mathf.Pow(distanceToPlayer,2) - Mathf.Pow(Mathf.Abs(transform.localPosition.x),2));
        var x = transform.localPosition.x;
        if (x > (distanceToPlayer * Mathf.Sqrt(2))/2)
        {
            x = (distanceToPlayer * Mathf.Sqrt(2))/2;
        }
        else if (x < -(distanceToPlayer * Mathf.Sqrt(2))/2)
        {
            x = -(distanceToPlayer * Mathf.Sqrt(2))/2;
        }

        if (z < (distanceToPlayer * Mathf.Sqrt(2))/2)
        {
            z = (distanceToPlayer * Mathf.Sqrt(2))/2;
        }
        transform.localPosition = new Vector3(x, transform.localPosition.y, z);
        
        if (oldPos != transform.localPosition && !finger)
        {
            elapsedTime += Time.deltaTime/10;
            if (elapsedTime >= 1f)
            {
                transform.localPosition = oldPos;
                elapsedTime = 0;
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, oldPos,elapsedTime);
        }
        else
        {
            elapsedTime = 0;
        }
    }


    public void OnFingerDown()
    {
        finger = true;
    }

    public void OnFingerUp()
    {
        finger = false;
    }
}
