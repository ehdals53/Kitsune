using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public GameObject attackIndicator;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public Camera cam;
    internal NavMeshAgent agent;

    PlayerBehaviour pb;

    private void Awake()
    {
        pb = GetComponent<PlayerBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        agent.updateRotation = false;
        attackIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(1))
        {
            LocatePosition();
        }

        if((Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance) || Input.GetKeyDown(KeyCode.S))
        {
            StopMove();
        }

        SetRotation();
    }
    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                //Debug.Log("Move");
                SetDestination(hit.point);
            }
        }
    }
    public void SetDestination(Vector3 destination)
    {
        StartMove();
        agent.speed = moveSpeed;
        agent.SetDestination(destination);
        pb.isAttacking = false;
        agent.stoppingDistance = 0;
        Cursor.SetCursor(GameManager.instance.cursers[0], Vector2.zero, CursorMode.Auto);
        pb.anim.SetBool(AnimatorParameters.hashAttacking, false);
        pb.anim.ResetTrigger(AnimatorParameters.hashAttack);


    }


    public void SetRotation()
    {
        if (!agent.isStopped)
        {
            Vector3 lookrotation = agent.steeringTarget - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), rotationSpeed * Time.deltaTime);

        }
    }
    public void StopMove()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        pb.anim.SetBool(AnimatorParameters.hashMove, false);
    }

    public void StartMove()
    {
        agent.isStopped = false;
        pb.anim.SetBool(AnimatorParameters.hashMove, true);
    }
}
