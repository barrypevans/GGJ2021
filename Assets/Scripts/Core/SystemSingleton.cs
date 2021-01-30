using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSingleton<T> : MonoBehaviour where T : SystemSingleton<T>
{
    static protected T g_instance = null;
    public static T Get()
    {
        Init();
        return g_instance;
    }

    public static void Init()
    {
        if (g_instance) return;

        System.Type singletonType = typeof(T);
        new GameObject(singletonType.ToString(), singletonType);
    }

    protected virtual void Awake()
    {
        if (g_instance != null)
        {
            // If there is already and instance created,
            // Destory this extra instance
            Destroy(gameObject);
            return;
        }
        else
        {
            g_instance = (T)this;
        }
    }
}
