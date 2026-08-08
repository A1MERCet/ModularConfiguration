using System;
using UnityEngine;
using UnityEngine.Events;

public class TimeToDestory : MonoBehaviour
{
    public float time = 1F;
    [HideInInspector] public float tick = 0F;
    public UnityEvent onDestroy = new();

    private void Start()
    {
        tick = time;
    }

    void Update()
    {
        tick-=Time.deltaTime;
        if (tick <= 0F) Destroy(gameObject);
    }

    private void OnDestroy() => onDestroy?.Invoke();
}
