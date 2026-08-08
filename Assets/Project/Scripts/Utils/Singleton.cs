using System.Reflection;

public abstract class Singleton<T> where T : new()
{
    private static T _instance;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                MethodInfo methodInfo = _instance.GetType().GetMethod("InitSingleton",BindingFlags.NonPublic | BindingFlags.Instance);
                methodInfo?.Invoke(_instance, null);
            }
            return _instance;
        }
    }
}