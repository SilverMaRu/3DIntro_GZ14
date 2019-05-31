using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    private Dictionary<object, Type> actionDic = new Dictionary<object, Type>();
    // Start is called before the first frame update
    protected virtual void Start()
    {
        MonoBehaviour[] monoBehaviours = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour tempMono in monoBehaviours)
        {
            actionDic.Add(tempMono, tempMono.GetType());
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
