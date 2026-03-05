using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandBarrier : MonoBehaviour
{
    private Collider _collider;
    public float redirectForce;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }


    private void OnTriggerStay(Collider other)
    {
        other.TryGetComponent<Rigidbody>(out var rb);
        if (rb != null)
        {
            Debug.Log("collision detected within island." + other.gameObject.name);
            Vector3 awayDir = (other.transform.position - transform.position).normalized;
            rb.AddForce(awayDir * redirectForce);
        }
    }
    
}
