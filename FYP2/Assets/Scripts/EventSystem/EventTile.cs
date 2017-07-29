using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ByteSheep.Events;

public class EventTile : MonoBehaviour
{
    public float timer = 0;
    private int currTimer;
    public bool persistent = false;
    
    public AdvancedEvent[] Events;
    private Queue<AdvancedEvent> eventsQueue;
    private void Start()
    {
        eventsQueue = new Queue<AdvancedEvent>();
        for(int i = 0; i < Events.Length; i++)
        {
            eventsQueue.Enqueue(Events[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            this.InvokeRepeating("InvokeNextEvent", 0, timer);
    }

    private void InvokeNextEvent()
    {
        if (eventsQueue.Count != 0)
        {
            AdvancedEvent advEvent = eventsQueue.Dequeue();
            advEvent.Invoke();
        }
        if (eventsQueue.Count == 0 && !persistent)
            this.gameObject.SetActive(false);
        else if (persistent)
            this.Start();
    }
}
