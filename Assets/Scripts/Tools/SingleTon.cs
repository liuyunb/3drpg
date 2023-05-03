using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : SingleTon<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    public static bool isInitialized { get { return instance != null; } }

    //public virtual void OnDestroy()
    //{
    //    if (instance != null)
    //    {
    //        instance = null;
    //        print(111);
    //    }
    //}
}
