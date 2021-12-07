using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XObjects : MonoBehaviour
{
    [SerializeField] private string xCount;
    [SerializeField] private PlayerTargetFollower player;
    private void Start()
    {
        player.GetJumpPositions(xCount,transform.position);
    }
}
