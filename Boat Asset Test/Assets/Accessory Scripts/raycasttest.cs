using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class raycasttest : MonoBehaviour
{

    public LayerMask layerMask;
    public Ray outRay;
    public RaycastHit hitInfo;
    private float normalLength = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
        
        

    }

    // Update is called once per frame
    void Update()
    {

        if (Physics.Raycast(transform.position, -transform.up, out hitInfo, Mathf.Infinity, layerMask))
        {
            //Debug.Log("hit water");
            outRay = new Ray(transform.position, -transform.up);
        }

        Vector3 start = hitInfo.point;
        Vector3 end = start + hitInfo.normal * normalLength;
        //Debug.Log("angle is : " + Vector3.Angle(start,end));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(outRay);


        Vector3 start = hitInfo.point;
        Vector3 end = start + hitInfo.normal * normalLength;

        Gizmos.DrawLine(start, end);
    }
}
