using UnityEngine;

public class BehaviourMWF: MonoBehaviour
{
    private MWFType type;
    public MWFType Type => type;
    
    private GLBScene glbScene;
    public GLBScene GLBScene => glbScene;

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
    
    public virtual void SetConfig(MWFType type, GLBScene glbScene)
    {
        this.type = type;
        this.glbScene = glbScene;
    }
}