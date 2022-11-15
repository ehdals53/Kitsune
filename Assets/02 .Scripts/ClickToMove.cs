using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotSpeed = 10f;
    private bool isMove;

    public Camera cam;
    internal NavMeshAgent agent;
    private Vector3 destination;

    PlayerBehaviour pb;

    private void Awake()
    {
        pb = GetComponent<PlayerBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pb.lockMovement && !pb.lockRotation)
        {
            if (Input.GetMouseButton(1))
            {
                LocatePosition();
            }
            LookMoveDirection();

        }

    }
    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider.tag != "Player")
            {
                SetDestination(hit.point);
            }
        }
        Debug.Log(hit.point);
    }
    private void SetDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
        destination = dest;
        isMove = true;
        agent.isStopped = false;
        pb.anim.SetFloat(AnimatorParameters.hashSpeed, 1f);


    }
    private void LookMoveDirection()
    {
        if (isMove)
        {
            if (agent.velocity.magnitude == 0f)
            {
                isMove = false;
                pb.anim.SetFloat(AnimatorParameters.hashSpeed, 0f);
                return;
            }

            var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
            pb.anim.transform.forward = dir;
        }
    }
    public void Stop()
    {
        isMove = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        pb.anim.SetFloat(AnimatorParameters.hashSpeed, 0f);
    }


}
