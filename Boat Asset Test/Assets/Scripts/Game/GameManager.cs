using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameEvent onAttemptComplete;

    private float currentAttemptTime;

    public Vector3 maxBoundary;

    private Vector3 boundaryCentre;

    public GameObject player;

    public GameEvent outOfBounds;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        boundaryCentre = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
    }

    public void RestartLevel(Component sender, object data)
    {
        SceneManager.LoadScene("Asset Test Scene");
    }

    public void EndAttempt(Component sender, object data)
    {
        if(data is object[] array)
        {
            data = array[0];
            if(data is float time)
            {
                currentAttemptTime = time;
            }
        }
        onAttemptComplete.Announce(this,currentAttemptTime);
    }


    public void TrackPlayer()
    {
        
        if(player.transform.position.x > boundaryCentre.x + maxBoundary.x || player.transform.position.x < boundaryCentre.x - maxBoundary.x ||
           player.transform.position.y > boundaryCentre.y + maxBoundary.y || player.transform.position.y < boundaryCentre.y - maxBoundary.y ||
           player.transform.position.z > boundaryCentre.z + maxBoundary.z || player.transform.position.z < boundaryCentre.z - maxBoundary.z)
        {
            outOfBounds.Announce(this, null);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, maxBoundary * 2);
    }
}
