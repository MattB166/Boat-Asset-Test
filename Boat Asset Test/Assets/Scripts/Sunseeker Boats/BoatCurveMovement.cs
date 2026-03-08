using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Script for the obstacle boats to follow a curved path, hopefully can work out how to do bezier curves, never used them before. 
/// </summary>
public class BoatCurveMovement : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform controlPoint;

    public float speed;
    private float t;
    private Rigidbody rb;
    public float closeEnoughDistance = 1f; 
    private bool shipShouldMove = true;

    //seems online I need two control points, and a start and end point. https://stackoverflow.com/questions/4034013/how-to-calculate-a-bezier-curve-with-only-start-and-end-points
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveShip();
    }


    Vector3 GetNextPointOnCurve(float t, Vector3 start, Vector3 control, Vector3 end)
    {
        return Mathf.Pow(1-t,2) * start + 2 * (1-t) * t * control + Mathf.Pow(t,2) * end;
    }

    private void OnDrawGizmos()
    {

        if(startPoint == null || endPoint == null || controlPoint == null)
        {
            return;
        }
        Vector3 prevPoint = startPoint.position;
        for(float t = 0; t <= 1; t += 0.01f)
        {
            Vector3 nextPoint = GetNextPointOnCurve(t, startPoint.position, controlPoint.position, endPoint.position);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    public void MoveShip()
    {
        if (shipShouldMove) 
        {
            float distanceToEndPoint = Vector3.Distance(transform.position, endPoint.position);
            
            if (distanceToEndPoint > closeEnoughDistance)
            {
                
                t += Time.deltaTime * speed;
                t = Mathf.Clamp(t,0,100);
                Vector3 nextPoint = GetNextPointOnCurve(t, startPoint.position, controlPoint.position, endPoint.position);
                rb.MovePosition(nextPoint); ////need to find a way i can use rb.addforce to make it more physics based and realistic. 

                Vector3 futurePoint = GetNextPointOnCurve(t + 0.01f, startPoint.position, controlPoint.position, endPoint.position);
                Vector3 direction = -(futurePoint - nextPoint).normalized;
                if (direction != Vector3.zero)
                {
                    float targetY = Quaternion.LookRotation(direction).eulerAngles.y;
                    Quaternion newRot = Quaternion.Euler(
                        transform.rotation.eulerAngles.x,
                        Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetY, Time.deltaTime * 5f),
                        transform.rotation.eulerAngles.z);
                    rb.MoveRotation(newRot);
                }

            }
            else
            {
                
                shipShouldMove = false;


            }

        }
        else
        {


        }


    }
}
