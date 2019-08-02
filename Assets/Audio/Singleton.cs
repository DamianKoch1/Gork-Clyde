﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Used to implement singletons that are attached to GameObjects
/// </summary>
/// <typeparam name="T">Type of class to make a singleton of</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<T>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }


    protected virtual void Start()
    {
        if (instance)
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }
}
