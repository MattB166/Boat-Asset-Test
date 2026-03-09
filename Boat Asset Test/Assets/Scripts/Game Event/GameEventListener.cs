using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Part of the custom event class I first used during FMP and use regularly in my projects to simplify event handling, increase abstraction and reduce coupling. 
/// Allows me to ping events with any data i want and have responses to those events in any script without needing to reference the script that is pinging the event, and without needing to know what data is being sent with the event.
/// </summary>
[System.Serializable]
public class CustomEvent : UnityEvent<Component, object> { }
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public CustomEvent response;

    private void OnEnable()
    {
        if (gameEvent != null)
            gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent != null)
            gameEvent.UnregisterListener(this);
    }

    public void Init(GameEvent gameEvent, UnityAction<Component,object> response)
    {
        this.gameEvent = gameEvent;
        this.response = new CustomEvent();
        this.response.AddListener(response);
        gameEvent.RegisterListener(this);
    }

    public void OnEventAnnounced(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
