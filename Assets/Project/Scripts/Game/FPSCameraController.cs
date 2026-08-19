using Project;
using UnityEngine;

public class FPSCameraController: SingletonMono<FPSCameraController>
{
    public Camera camera;
    public IncrementPos increment = new ();
    public Increment incrementFOV = new ();

    protected override void Awake()
    {
        base.Awake();
        if (incrementFOV.defaultValue == 0F) incrementFOV.defaultValue = camera.fieldOfView;
    }

    private void Update()
    {
        increment.Count();
        var incrementCache = increment.Cache;
        camera.transform.localPosition = incrementCache.position;
        camera.transform.localRotation = Quaternion.Euler(incrementCache.rotation);

        incrementFOV.Count();
        camera.fieldOfView = incrementFOV.Cache;
    }
}