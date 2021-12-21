using UnityEngine;
using System;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    public static T Instance {
        get
        {
            if(instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }
            return instance;
        }
    }

    virtual protected void Awake()
    {
        if(this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
