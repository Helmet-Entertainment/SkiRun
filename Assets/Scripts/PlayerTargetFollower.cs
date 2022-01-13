using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Lean.Touch;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTargetFollower : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Animator playerAnimator;
    private int flagCount = 0;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform playerGraphic;
    [SerializeField] private CinemachineVirtualCamera followCam;
    [SerializeField] private LeanMultiSet multiSet;
    [SerializeField] private CinemachineVirtualCamera oldCam;
    [SerializeField] private ParticleSystem leaf1, leaf2;
    [SerializeField] private CinemachineVirtualCamera winCam;
    [SerializeField] private ParticleSystem winParticle;
    [SerializeField] private Vector3 directionA;
    private Dictionary<string,Vector3> jumpPositions = new Dictionary<string, Vector3>();
    private bool hasFinished;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        multiSet.enabled = false;
    }

    private void Update()
    {
        if (hasFinished)
        {
            return;
        }
        //Vector3 targetPos = ((transform.position + Vector3.forward) + target.position);
        Vector3 targetPos = target.position;
        agent.destination = targetPos;
        DrawLine(agent.destination);
        RotatePlayerToFloor();
        UpdateLookRotation();
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


    private void DrawLine(Vector3 endPos)
    {
        Debug.DrawLine(transform.position,endPos,Color.black,20f);
    }
    public void UpdateLookRotation()
    {
        var rotation = playerGraphic.rotation;
        playerGraphic.LookAt(agent.destination);
        var rotationForY = playerGraphic.rotation;
        rotation.y = rotationForY.y;
        playerGraphic.rotation = rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flag"))
        {
            SpeedUp();
            flagCount++;
            other.GetComponent<Animator>().SetTrigger("FlagDown");
        }
        else if (other.CompareTag("StartTigger"))
        {
            oldCam.Priority = 0;
            multiSet.enabled = true;
        }
        else if (other.CompareTag("LevelEnd"))
        {
            Jump();
        }
    }

    [ContextMenu("Speed")]
    private void SpeedUp()
    {
        var transposer = followCam.GetCinemachineComponent<CinemachineTransposer>();
        StartCoroutine(SpeedUpLerp(transposer,
            new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y + 2, transposer.m_FollowOffset.z - 2.5f),
            agent.speed,
            agent.speed + 5));
    }

    [ContextMenu("Slow")]
    private void SlowDown()
    {
        var transposer = followCam.GetCinemachineComponent<CinemachineTransposer>();
        StartCoroutine(SlowDownLerp(transposer,
            new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y - 2, transposer.m_FollowOffset.z + 2.5f),
            agent.speed,
            agent.speed - 5));
    }
    
    public void HitObstacle()
    { 
        //playerAnimator.SetTrigger("TakeDamage");
        Fail();
    }


    [ContextMenu("Jump")]
    private void Jump()
    {
        leaf1.Stop();
        leaf2.Stop();
        playerAnimator.SetTrigger("Flying");
        agent.enabled = false;
        hasFinished = true;
        if (flagCount == 0)
        {
            flagCount = 1;
        }
        transform.DOJump(jumpPositions[$"{flagCount}x"],4*flagCount,1,flagCount).SetEase(Ease.Linear);
        StartCoroutine(WaitForJump(flagCount));
    }

    public void GetJumpPositions(string xCount, Vector3 position)
    {
        jumpPositions.Add(xCount,position);
    }

    private void Fail()
    {
        if (!hasFinished)
        {
            hasFinished = true;
            agent.destination = transform.position;
            playerAnimator.SetTrigger("FailStone");
            FindObjectOfType<UIManager>().EnableLosePanel();
            agent.enabled = false;
            this.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(RotatePlayer());
            leaf1.Stop();
            leaf2.Stop();
        }
    }

    IEnumerator RotatePlayer()
    {
        float elapsedtime = 0f;
        while (elapsedtime<1f)
        {
            elapsedtime += Time.deltaTime;
            transform.rotation=Quaternion.Lerp(transform.rotation,Quaternion.Euler(45,0,0), elapsedtime);
            yield return new WaitForEndOfFrame();
        }
        
    }
    
    IEnumerator WaitForJump(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time-0.3f)
        {
            elapsedTime += Time.deltaTime;
            this.transform.rotation = Quaternion.Euler(0,0,0);
            yield return new WaitForEndOfFrame();
        }
        
        playerAnimator.SetTrigger("Flip");
        FindObjectOfType<UIManager>().EnableWinPanel();
        RotatePlayerToFloor();
        winCam.Priority = 13;
        winParticle.Play();
    }

    IEnumerator SpeedUpLerp(CinemachineTransposer transposer,Vector3 newOffset, float oldSpeed, float newSpeed)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            agent.speed = Mathf.Lerp(oldSpeed,newSpeed, elapsedTime*2);
            //targetAgent.speed = Mathf.Lerp(oldSpeed,newSpeed, elapsedTime*2);
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset,newOffset, elapsedTime);
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator SlowDownLerp(CinemachineTransposer transposer,Vector3 newOffset, float oldSpeed, float newSpeed)
    {
        if (oldSpeed <= 25)
        {
            oldSpeed = 25f;
        }

        if (newOffset.y < 5 || newOffset.z > -5.72f)
        {
            newOffset.y = 5;
            newOffset.z = -5.72f;
        }
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime/2;
            agent.speed = Mathf.Lerp(oldSpeed,newSpeed, elapsedTime*2);
            //targetAgent.speed = Mathf.Lerp(oldSpeed,newSpeed, elapsedTime*2);
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset,newOffset, elapsedTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
