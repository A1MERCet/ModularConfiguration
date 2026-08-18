using UnityEngine;

public class BehaviourMWF: MonoBehaviour
{
    private MWFType config;
    public MWFType Config => config;
    
    public GameObject model;
    public Animation _animation;
    public Animation Animation => _animation;

    protected virtual void Awake()
    {
        
    }
    
    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {
        
    }
    
    protected virtual void FixedUpdate()
    {
        
    }
    
    public virtual void SetConfig(MWFType type, GameObject model)
    {
        this.config = type;
        this.model = model;
        _animation = this.model?.GetComponentInChildren<Animation>();
    }
}