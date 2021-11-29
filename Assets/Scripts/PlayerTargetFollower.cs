using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTargetFollower : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform playerGraphic;
    private float velocity = 20f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = target.position;
        RotatePlayerToFloor();
    }

    private void RotatePlayerToFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position+Vector3.up, transform.TransformDirection(Vector3.down), out hit, 10f, groundLayerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            playerGraphic.up = hit.normal;
            var rotation = playerGraphic.rotation;
            rotation.y = this.transform.rotation.y;
            playerGraphic.rotation = rotation;
        }
    }
}
