using System.Collections.Generic;
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

    private Dictionary<string, List<Transform>> _nodeCache = new();

    protected virtual void Awake()
    {
        incrementPos.Add("showcase");
    }
    
    protected virtual void Start()
    {
        UpdateNodeCache();
    }

    public virtual void UpdateNodeCache()
    {
        _nodeCache.Clear();
        foreach (Transform child in UtilUnity.GetChildren(transform))
        {
            if (!_nodeCache.ContainsKey(child.name)) _nodeCache.Add(child.name, new List<Transform>());
            _nodeCache[child.name].Add(child);
        }
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

    public Transform GetNode(string name) => _nodeCache.ContainsKey(name) ? _nodeCache[name][0] : transform.Find(name);
    public Transform GetNodeOrDefault(string name) => GetNodeOrDefault(name, model.transform);
    public Transform GetNodeOrDefault(string name, Transform def)
    {
        if (_nodeCache.ContainsKey(name)) return _nodeCache[name][0];
        var find = transform.Find(name);
        if (find) return find;
        return def;
    }

    public bool HasNode(string name) => _nodeCache.ContainsKey(name) || transform.Find(name) != null;
    
    public void HideInChildrenStartsWith(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name.StartsWith(name))
                c.gameObject.SetActive(false);
    }
    public void ShowInChildrenStartsWith(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name.StartsWith(name))
                c.gameObject.SetActive(true);
    }
    
    public void HideInChildrenEndsWith(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name.EndsWith(name))
                c.gameObject.SetActive(false);
    }
    public void ShowInChildrenEndsWith(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name.EndsWith(name))
                c.gameObject.SetActive(true);
    }
    
    public void HideInChildren(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name == name)
                c.gameObject.SetActive(false);
    }
    
    public void ShowInChildren(string name)
    {
        foreach (var c in UtilUnity.GetChildren(transform))
            if (c.name == name)
                c.gameObject.SetActive(true);
    }
    
    public void HideInChildren(string[] name)
    {
        if (name == null) return;
        foreach (var c in UtilUnity.GetChildren(transform))
            for (var i = 0; i < name.Length; i++)
                if (c.name == name[i])
                    c.gameObject.SetActive(false);
    }
    
    public void ShowInChildren(string[] name)
    {
        if (name == null) return;
        foreach (var c in UtilUnity.GetChildren(transform))
            for (var i = 0; i < name.Length; i++)
                if (c.name == name[i])
                    c.gameObject.SetActive(true);
    }
    
    public void HideInChildren(List<string> name)
    {
        if (name == null) return;
        foreach (var c in UtilUnity.GetChildren(transform))
            if (name.Contains(c.name))
                c.gameObject.SetActive(false);
    }
    public void ShowInChildren(List<string> name)
    {
        if (name == null) return;
        foreach (var c in UtilUnity.GetChildren(transform))
            if (name.Contains(c.name))
                c.gameObject.SetActive(true);
    }
}