using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandDocking : MonoBehaviour
{
    private Collider _dockingCollider;
    public GameEvent onBoatEntered;
    public GameEvent onBoatExited;

    // Start is called before the first frame update
    void Start()
    {
        _dockingCollider = GetComponent<Collider>();
        _dockingCollider.isTrigger = true; 
        if (_dockingCollider == null)
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
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Boat exited docking area.");
            if(onBoatExited != null)
                onBoatExited.Announce(this);
        }
    }

    public void OnCompletion()
    {
        transform.GetComponent<Collider>().enabled = false;
        SceneManager.LoadScene("Asset Test Scene");  ///in a temp place until UI has been set up. 
    }
}
