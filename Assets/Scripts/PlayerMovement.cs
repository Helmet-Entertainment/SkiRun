using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float conSpeed = 5, horSpeed = 5;
    public Rigidbody rb;
    public LayerMask inputLayer;
    
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
        TakeInput();
        curSpeed = Mathf.SmoothDamp(curSpeed, conSpeed, ref smoothSpeedVelo, .7f);
    }

    private void TakeInput()
    {
        //movePos = transform.forward * curSpeed;
        bool mousePressed = Input.GetMouseButton(0);
        if (mousePressed)
        {
            RaycastHit hit;
            Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit, 25f, inputLayer))
            {
                inputPos = hit.point;
                inputPos = new Vector3(inputPos.x * horSpeed, 0, 0);
                //movePos = new Vector3(inputPos.x,movePos.y,movePos.z);
            }
        }
        //movePos = Vector3.SmoothDamp(transform.position, inputPos, ref smoothVelo, smoothTime);
        movePos = inputPos - new Vector3(transform.position.x,0, 0) + transform.forward * curSpeed;
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
