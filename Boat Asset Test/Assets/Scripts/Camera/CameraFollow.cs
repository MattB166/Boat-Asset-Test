using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public float smoothRotate = 3f;
    public Vector3 offset;

    private void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 desiredPos = target.TransformPoint(offset);
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;

        Quaternion desiredRot = Quaternion.LookRotation(target.forward,Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, smoothRotate * Time.deltaTime);
    }
}
