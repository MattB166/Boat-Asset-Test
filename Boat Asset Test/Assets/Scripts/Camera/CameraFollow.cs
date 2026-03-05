using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public float smoothRotate = 3f;
    public Vector3 startOffset;

    private void Start()
    {
    }

    void LateUpdate()
    {
        Vector3 desiredPos = target.TransformPoint(startOffset.x,startOffset.y,startOffset.z);
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        smoothPos.y = transform.position.y;
        transform.position = smoothPos;

        Quaternion desiredRot = Quaternion.LookRotation(target.forward,Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, smoothRotate * Time.deltaTime);
    }
}
