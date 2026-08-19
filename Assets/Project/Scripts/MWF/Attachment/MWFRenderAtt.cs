using UnityEngine;

public class MWFRenderAtt: MWFRenderOBJ
{
    public override string GetConfigType() => "attachments";
    protected override BehaviourMWF PostLoadOBJ(GameObject o) => o.AddComponent<BehaviourMWFAtt>();
}