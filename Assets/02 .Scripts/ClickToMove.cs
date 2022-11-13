using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public Camera cam;
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;
    private Vector3 position;
    private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        cam = Camera.main;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            LocatePosition();
        }
        MoveToPosition();
    }

    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
    }
    void MoveToPosition()
    {
        if (Vector3.Distance(transform.position, position) > 1)
        {
            Quaternion rot = Quaternion.LookRotation(position - transform.position, Vector3.forward);

            rot.x = 0f;
            rot.z = 0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
            cc.SimpleMove(transform.forward * moveSpeed);
        }
    }
}
