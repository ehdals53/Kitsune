using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public Camera cam;
    public float speed;
    Vector3 position;
    

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
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
        Quaternion rot = Quaternion.LookRotation(position - transform.position, Vector3.forward);

       // transform
    }
}
