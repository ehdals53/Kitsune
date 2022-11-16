using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    public Camera cam;
    internal NavMeshAgent agent;

    PlayerBehaviour pb;

    [SerializeField] private GameObject clickMarkerPrefab;
    [SerializeField] private Transform visualObjectsParent;

    private void Awake()
    {
        pb = GetComponent<PlayerBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            LocatePosition();
        }

        if(Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance)
        {
            clickMarkerPrefab.transform.SetParent(transform);
            clickMarkerPrefab.SetActive(false);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            pb.anim.SetFloat(AnimatorParameters.hashSpeed, 0);
        }

    }
    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            SetDestination(hit.point);

            //if (hit.collider.tag != "Player")
            //{
            //}
        }
        //Debug.Log(hit.point);
    }
    private void SetDestination(Vector3 dest)
    {
        clickMarkerPrefab.SetActive(true);
        clickMarkerPrefab.transform.SetParent(visualObjectsParent);
        clickMarkerPrefab.transform.position = dest;
        agent.speed = moveSpeed;
        agent.SetDestination(dest);
        agent.isStopped = false;
        pb.anim.SetFloat(AnimatorParameters.hashSpeed, 1f);

    }
    public void StopMove()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        pb.anim.SetFloat(AnimatorParameters.hashSpeed, 0f);
    }

    public void StartMove()
    {
        agent.isStopped = false;
    }


}
