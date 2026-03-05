using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
