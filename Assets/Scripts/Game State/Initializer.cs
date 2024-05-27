using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    
    public static void Execute()
    {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("SHARED")));
    }
}
