using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// controls status and progression of the docking objective. holds info like how far away the player is. 
/// </summary>
public class DockingObjective : MonoBehaviour
{
    private float _distanceToPlayer;
    [HideInInspector] public float objectiveTimer;
    [HideInInspector] public float dockingTimer;
    public float dockingTimeRequired;
    private bool isObjectiveActive;
    private bool isDocking;
    private bool isDockingComplete = false;
    public GameEvent onDockingComplete;
    public GameEvent onDockingStopped;
    public GameEvent objectiveTimeChanged;
    public GameEvent onDockingTimeChanged;
    public float timeToWaitAfterCompletion = 2f;

    // Start is called before the first frame update
    void Start()
    {
        isObjectiveActive = true;
        objectiveTimer = 0;
        dockingTimer = dockingTimeRequired;
    }

    // Update is called once per frame
    void Update()
    {
        if(isObjectiveActive)
        {
            ObjectiveCount();
        }
        if (isDocking && !isDockingComplete)
        {
            Dock();
        }
    }


    public void Dock()
    {
        dockingTimer -= Time.deltaTime;
        onDockingTimeChanged.Announce(this, dockingTimer);
        if (dockingTimer <= 0)
        {
            Debug.Log("Docking complete!");
            if(onDockingComplete != null)
                isObjectiveActive = false;
            
            isDockingComplete = true;
            isDocking = false;
            
            StartCoroutine(EndOfAttempt());
        }
    }

    public void ObjectiveCount()
    {
        objectiveTimer += Time.deltaTime;
        if(objectiveTimeChanged != null)
            objectiveTimeChanged.Announce(this,objectiveTimer);
        
    }

    public void StopDocking()
    {
        isDocking = false;
        if(onDockingStopped != null)
            onDockingStopped.Announce(this);
        dockingTimer = dockingTimeRequired;
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

    private IEnumerator EndOfAttempt()
    {
        yield return new WaitForSeconds(timeToWaitAfterCompletion);
        
        Debug.Log("Announcing docking complete.");
        onDockingComplete.Announce(this, objectiveTimer);
        objectiveTimer = 0;
        
    }
}
