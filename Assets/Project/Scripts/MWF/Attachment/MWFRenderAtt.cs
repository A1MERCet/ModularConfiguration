using UnityEngine;

public class MWFRenderAtt: MWFRenderOBJ
{
    public override string GetConfigType() => "attachment";
    protected override BehaviourMWF PostLoadOBJ(GameObject o) => o.AddComponent<BehaviourMWFAtt>();
}