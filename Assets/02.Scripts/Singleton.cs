using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _intance;

    public static T Instance
    {
        get
        {
            if (_intance == null)
            {
                _intance = FindObjectOfType<T>();
                if (_intance == null)
                {
                    GameObject sigleton = new GameObject(typeof(T).Name);
                    _intance = sigleton.AddComponent<T>();
                }
            }
            return _intance;
        }
    }

    protected virtual void Awake()
    {
        if (_intance == null)
        {
            _intance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
