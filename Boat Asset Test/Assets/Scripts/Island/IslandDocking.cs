using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandDocking : MonoBehaviour
{
    private Collider dockingCollider;
    public GameEvent onBoatEntered;
    public GameEvent onBoatExited;
    private GameObject dockingZoneIndicator;

    // Start is called before the first frame update
    void Start()
    {
        dockingZoneIndicator = transform.GetComponentInChildren<EndZonePulse>().gameObject;
        dockingCollider = GetComponent<Collider>();
        dockingCollider.isTrigger = true; 
        if (dockingCollider == null)
        {
            Debug.LogError("No collider found on IslandDocking object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Boat entered docking area.");
            if(onBoatEntered != null)
                onBoatEntered.Announce(this);
            dockingZoneIndicator.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Boat exited docking area.");
            if(onBoatExited != null)
                onBoatExited.Announce(this);
                dockingZoneIndicator.gameObject.SetActive(true);
        }
    }

    public void OnCompletion()
    {
        transform.GetComponent<Collider>().enabled = false;
        
    }
}
