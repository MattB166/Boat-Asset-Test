using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameEvent onAttemptComplete;

    private float _currentAttemptTime;

    public Vector3 maxBoundary;

    private Vector3 boundaryCentre;

    public GameObject player;

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
                _currentAttemptTime = time;
            }
        }
        onAttemptComplete.Announce(this,_currentAttemptTime);
    }


    public void TrackPlayer()
    {
        ///create a boundary box using the maxBoundary variable and the player's position. If the player goes outside of the boundary box, restart the level
        
        if(player.transform.position.x > boundaryCentre.x + maxBoundary.x || player.transform.position.x < boundaryCentre.x - maxBoundary.x ||
           player.transform.position.y > boundaryCentre.y + maxBoundary.y || player.transform.position.y < boundaryCentre.y - maxBoundary.y ||
           player.transform.position.z > boundaryCentre.z + maxBoundary.z || player.transform.position.z < boundaryCentre.z - maxBoundary.z)
        {
            RestartLevel(this, null);
        }
    }
}
