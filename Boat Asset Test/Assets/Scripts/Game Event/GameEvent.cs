using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    public void Announce(Component sender, params object[] data)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventAnnounced(sender, data);

        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if(listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
