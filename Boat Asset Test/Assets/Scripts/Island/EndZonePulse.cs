using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZonePulse : MonoBehaviour
{

    public float speed = 2f;
    public float scaleAmount = 0.5f;

    private float yScale;
    // Start is called before the first frame update
    void Start()
    {
        yScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * speed) * scaleAmount;

        transform.localScale = new Vector3(transform.localScale.x, yScale * scale, transform.localScale.z);
    }
}
