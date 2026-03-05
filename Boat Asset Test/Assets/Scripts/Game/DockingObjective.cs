using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// controls status and progression of the docking objective. holds info like how far away the player is. 
/// </summary>
public class DockingObjective : MonoBehaviour
{
    private float _distanceToPlayer;
    [HideInInspector] public float dockingTimer;
    public float dockingTimeRequired;
    private bool isDocking;
    private bool isDockingComplete = false;
    public GameEvent onDockingComplete;
    public GameEvent onDockingStopped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDocking && !isDockingComplete)
        {
            Dock();
        }
    }


    public void Dock()
    {
        dockingTimer += Time.deltaTime;
        Debug.Log("Timer still ticking...");
        Debug.Log("Docking time" + dockingTimer);
        if (dockingTimer >= dockingTimeRequired)
        {
            Debug.Log("Docking complete!");
            if(onDockingComplete != null)
                onDockingComplete.Announce(this);
            isDockingComplete = true;
            isDocking = false;
        }
    }

    public void StopDocking()
    {
        isDocking = false;
        if(onDockingStopped != null)
            onDockingStopped.Announce(this);
        dockingTimer = 0;
        Debug.Log("Docking stopped from within objective.");
    }

    public void StartDocking()
    {
        isDocking = true;
    }
     public float GetDistanceToPlayer()
    {
        return _distanceToPlayer;
    }
     public void SetDistanceToPlayer(float distance)
    {
        _distanceToPlayer = distance;
    }
}
