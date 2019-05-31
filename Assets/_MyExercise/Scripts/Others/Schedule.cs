using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;

public class Schedule
{
    private System.Action method;
    private float time;
    private bool isLoop;
    private bool loop;
    private Thread workThread;

    public Schedule(System.Action method, float time) : this(method, time, false) { }

    public Schedule(System.Action method, float time, bool isLoop)
    {
        this.method = method;
        this.time = time;
        this.isLoop = isLoop;
        if (isLoop)
        {
            workThread = new Thread(LoopThreadStart);
            workThread.Start();
        }
        else
        {
            workThread = new Thread(UnloopThreadStart);
            workThread.Start();
        }
        
    }

    private void LoopThreadStart()
    {
        loop = true;
        float lastTime = 0;
        while (loop)
        {
            if(Time.time - lastTime >= time)
            {
                method?.Invoke();
            }
        }
        workThread.Abort();
    }

    private void UnloopThreadStart()
    {
        float startTime = Time.time;
        while (Time.time - startTime <= time) { }
        method?.Invoke();
        workThread.Abort();
    }

    public void EndLoop()
    {
        loop = false;
    }

    //public static void StartWork(Action method, float time)
    //{


    //}

    //public static void StartW
}
