using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float conSpeed = 5, horSpeed = 5;
    public Rigidbody rb;
    
    private Camera cam;
    private Vector3 movePos;
    private Vector3 inputPos;
    private float curSpeed;
    private Animator animator;

    private void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
        cam = Camera.main;
    }

    private Vector3 smoothVelo;
    public float smoothTime = 0.5f;

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movePos * Time.deltaTime);
    }

    private float smoothSpeedVelo;

    void Update()
    {
        movePos = transform.forward * curSpeed;
        curSpeed = Mathf.SmoothDamp(curSpeed, conSpeed, ref smoothSpeedVelo, .7f);
    }

    public void HitObstacle(float slowSpeed, int penalty)
    {
        int score = UIManager.instance.theScore;
        score -= penalty;
        if (score < 0)
        {
            score = 0;
        }

        UIManager.instance.theScore = score;
        UIManager.instance.UpdateScore();
        animator.SetTrigger("TakeDamage");
        curSpeed = slowSpeed;
    }
}
