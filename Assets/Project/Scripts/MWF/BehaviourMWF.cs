using Project;
using UnityEngine;

public class BehaviourMWF: MonoBehaviour
{
    private MWFType config;
    public MWFType Config => config;
    
    public GameObject model;
    public Animation _animation;
    public Animation Animation => _animation;
    public IncrementPos incrementPos = new  IncrementPos();
    public IncrementPos incrementPosModify = new IncrementPos();
    public bool updatePos = true;

    protected virtual void Awake()
    {
        incrementPos.Add("showcase");
    }
    
    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {

        if (model != null)
        {
            incrementPos.Count();
            incrementPosModify.Count();
            if (updatePos) {
                model.transform.localPosition = incrementPos.Cache.position + incrementPosModify.Cache.position;
                model.transform.localRotation = Quaternion.Euler(incrementPos.Cache.rotation + incrementPosModify.Cache.rotation);
            }else {
                model.transform.localPosition = incrementPosModify.Cache.position;
                model.transform.localRotation = Quaternion.Euler(incrementPosModify.Cache.rotation + new Vector3(0, 90, 0));
            }
        }
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