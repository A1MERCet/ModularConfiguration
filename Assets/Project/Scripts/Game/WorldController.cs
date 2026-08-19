using UnityEngine;

public class WorldController : SingletonMono<WorldController>
{
    public Transform shootingRange;

    protected override void Awake()
    {
        base.Awake();
        shootingRange.gameObject.SetActive(false);
    }
}
