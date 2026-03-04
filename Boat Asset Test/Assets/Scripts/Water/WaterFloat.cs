using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages the buoyancy of objects by tracking the positions of the floats to the simulated ocean layer. 
/// </summary>
public class WaterFloat : MonoBehaviour
{

    public float airDrag = 1;
    public float waterDrag = 2;
    public Transform[] floatPoints;

    protected Rigidbody rb;

    protected PlayerOcean ocean;

    protected float waterLine;
    protected Vector3[] waterlinePoints;

    protected Vector3 centreOffset;
    public bool attachToWaterline;

    public Vector3 Centre { get { return transform.position + centreOffset; } }

    // Start is called before the first frame update

    private void Awake()
    {
        ocean = GetComponentInChildren<PlayerOcean>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        waterlinePoints = new Vector3[floatPoints.Length];
        for (int i = 0; i < floatPoints.Length; i++)
        {
            waterlinePoints[i] = floatPoints[i].position;
            centreOffset = GetCentre(waterlinePoints) - transform.position;

        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < floatPoints.Length; i++)
        {
            Vector3 localPos = ocean.transform.InverseTransformPoint(floatPoints[i].position);
            waterlinePoints[i] = floatPoints[i].position;
            waterlinePoints[i].y = ocean.transform.position.y + ocean.GetHeight(localPos);
            //check if the point is underwater, and apply forces accordingly inside fixed update, to ensure the physics engine is applying the forces at the correct time, and not missing any frames where the point may be underwater.
            if (floatPoints[i].position.y < waterlinePoints[i].y)
            {
                Debug.Log("Point " + i + " is underwater");
            }
            else
            {
                Debug.Log("Point " + i + " is above water");
            }

        }
    }

    public Vector3 GetCentre(Vector3[] points)
    {
        var centre = Vector3.zero;
        for (int i = 0; i < points.Length; i++)
        {
            centre += points[i] / points.Length;

        }
        return centre;
    }

    private void FixedUpdate()
    {
        //apply very small upward force to the boat countering the amplitude of the waves, to prevent the boat from sinking when it should be floating on the water. this is done by calculating the average height of the water at the float points, and applying a force proportional to the difference between the boat's current height and the average water height. also apply additional forces at each float point based on how far underwater they are, to simulate buoyancy more accurately and create a more realistic interaction between the boat and the waves.
        //need to take into account the wave details so the boat doesnt sink. 
        bool anyUnderwater = false;
        float totalWaterY = 0f;

        for (int i = 0; i < floatPoints.Length; i++)
        {
            Vector3 localPos = ocean.transform.InverseTransformPoint(floatPoints[i].position);
            float waterY = ocean.transform.position.y + ocean.GetHeight(localPos);

            float displacement = waterY - floatPoints[i].position.y;
            if (displacement > 0)
            {
                anyUnderwater = true;

                float force = displacement * 50f; // buoyancy force proportional to displacement
                rb.AddForceAtPosition(Vector3.up * force, floatPoints[i].position, ForceMode.Force);
            }
            totalWaterY += waterY;


        }

        float averageWaterY = totalWaterY / floatPoints.Length;
        float verticalDisplacement = averageWaterY - (rb.position.y + centreOffset.y);
        //Vector3 targetPos = rb.position;
        //targetPos.y = Mathf.Lerp(rb.position.y,averageWaterY - centreOffset.y, Time.fixedDeltaTime * 2f);
        //rb.MovePosition(targetPos);
        rb.AddForce(Vector3.up * verticalDisplacement * 20f, ForceMode.Force);

        rb.drag = anyUnderwater ? waterDrag : airDrag;
        rb.angularDrag = anyUnderwater ? waterDrag : airDrag;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (floatPoints == null) return;

        for (int i = 0; i < floatPoints.Length; i++)
        {
            if (floatPoints[i] == null) continue;

            if (ocean != null)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawCube(waterlinePoints[i], Vector3.one * 0.5f);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(floatPoints[i].position, 0.1f);
        }




    }
}
