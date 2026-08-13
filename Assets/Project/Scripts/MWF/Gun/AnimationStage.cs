public struct AnimationStage
{
    public string name;
    public int startTime;
    public int endTime;
    public float speed;

    public override string ToString() => $"{name} {startTime}>{endTime}*({speed})"; 
}