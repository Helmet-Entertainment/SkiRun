using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //transform.position += transform.forward * 6f * Time.deltaTime;
        Vector3 targetPos = transform.position + Vector3.forward;
        agent.destination = targetPos;
    }
}
