using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThread: SingletonMono<MainThread>
{
    private Queue<Action> actions = new();

    private void Update()
    {
        if (actions.Count > 0)
        {
            var action = actions.Peek();
            actions.Dequeue();
            try {
                action?.Invoke();
            }catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
    
    public void Enqueue(Action a) => actions.Enqueue(a);
}