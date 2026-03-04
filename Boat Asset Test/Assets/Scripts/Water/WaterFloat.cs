using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloat : MonoBehaviour
{

    public float airDrag = 1;
    public float waterDrag = 10;
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
        //apply very small upward force to the boat, to simulate buoyancy, and prevent it from sinking through the water when the player gets out of the boat, or when the boat is stationary for a long time, as the physics engine may not apply enough force to keep it afloat.
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

                float force = displacement * 10f; // buoyancy force proportional to displacement
                rb.AddForceAtPosition(Vector3.up * force, floatPoints[i].position);
            }
            totalWaterY += waterY;


        }

        float averageWaterY = totalWaterY / floatPoints.Length;
        Vector3 targetPos = rb.position;
        targetPos.y = Mathf.Lerp(rb.position.y,averageWaterY - centreOffset.y, Time.fixedDeltaTime * 2f);
        rb.MovePosition(targetPos);

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
