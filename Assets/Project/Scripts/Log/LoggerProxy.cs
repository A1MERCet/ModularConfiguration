public class LoggerProxy
{
    public readonly string id;

    public LoggerProxy(string id)
    {
        this.id = id;
    }

    public void Info(string msg) => LoggerManager.instance.Info($"[{id}]{msg}");
    public void Warn(string msg) => LoggerManager.instance.Warn($"[{id}]{msg}");
    public void Error(string msg) => LoggerManager.instance.Error($"[{id}]{msg}");
    
}