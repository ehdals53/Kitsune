using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiggleBonePhysics : MonoBehaviour
{
    /// <summary>
    /// �θ� ���� Ʈ������
    /// </summary>
    Transform parentTransform;
    /// <summary>
    /// �� ��ũ��Ʈ�� ���Ե� �� ������Ʈ�� ������ �ٵ�
    /// </summary>
    Rigidbody boneRigidbody;
    /// <summary>
    /// ���� �����ӱ����� �θ� ���� ��ġ
    /// </summary>
    Vector3 prevFrameParentPosition = Vector3.zero;
    /// <summary>
    /// ���� ����ġ
    /// </summary>
    public float power = 0f;
    /// <summary>
    /// ����� ��ġ�� ũ���� ����. ���� ���� �ʹ� ũ�� �� ���� ����� ������ ���ؼ�
    /// �� �������� �̻��� ��ġ�� ���ư��� ������ �߻��� �� �ִ�.
    /// </summary>
    public float clampDist = 0.03f;

    void Start()
    {
        parentTransform = transform.parent;
        prevFrameParentPosition = parentTransform.position;

        boneRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 delta = (prevFrameParentPosition - parentTransform.position);
        boneRigidbody.AddForce(Vector3.ClampMagnitude(delta, clampDist) * power);

        prevFrameParentPosition = parentTransform.position;
    }
}