using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Others
{
    public class EventManager
    {
        private static Dictionary<string, Delegate> delegateDic = new Dictionary<string, Delegate>();

        public static void DeclareEvent(string eventName)
        {
            if (!delegateDic.ContainsKey(eventName))
            {
                delegateDic.Add(eventName, null);
            }
        }

        private static void AddDelegate(string eventName, Delegate newAction)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue)
            {
                delegateDic[eventName] = Delegate.Combine(existingDelegate, newAction);
            }
            else
            {
                delegateDic.Add(eventName, newAction);
            }
        }

        public static void BindingEvent(string eventName, System.Action newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T>(string eventName, Action<T> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T1, T2>(string eventName, Action<T1, T2> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void BindingEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> newAction)
        {
            AddDelegate(eventName, newAction);
        }

        public static void OnEvent(string eventName)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                //existingDelegate.DynamicInvoke();
                Delegate[] delegates = existingDelegate.GetInvocationList();
                foreach (Delegate tempDelegate in delegates)
                {
                    try
                    {
                        tempDelegate.DynamicInvoke();
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        Delegate.Remove(existingDelegate, tempDelegate);
                    }
                }
            }
        }

        public static void OnEvent<T>(string eventName, T arg)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                //existingDelegate.DynamicInvoke(arg);
                Delegate[] delegates = existingDelegate.GetInvocationList();
                foreach (Delegate tempDelegate in delegates)
                {
                    try
                    {
                        tempDelegate.DynamicInvoke(arg);
                    }
                    catch(System.Reflection.TargetInvocationException)
                    {
                        Delegate.Remove(existingDelegate, tempDelegate);
                    }
                }
            }
        }

        public static void OnEvent<T1, T2>(string eventName, T1 arg0, T2 arg1)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                //existingDelegate.DynamicInvoke(arg0, arg1);
                Delegate[] delegates = existingDelegate.GetInvocationList();
                foreach (Delegate tempDelegate in delegates)
                {
                    try
                    {
                        tempDelegate.DynamicInvoke(arg0, arg1);
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        Delegate.Remove(existingDelegate, tempDelegate);
                    }
                }
            }
        }

        public static void OnEvent<T1, T2, T3>(string eventName, T1 arg0, T2 arg1, T3 arg2)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                //existingDelegate.DynamicInvoke(arg0, arg1, arg2);
                Delegate[] delegates = existingDelegate.GetInvocationList();
                foreach (Delegate tempDelegate in delegates)
                {
                    try
                    {
                        tempDelegate.DynamicInvoke(arg0, arg1, arg2);
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        Delegate.Remove(existingDelegate, tempDelegate);
                    }
                }
            }
        }

        public static void OnEvent<T1, T2, T3, T4>(string eventName, T1 arg0, T2 arg1, T3 arg2, T4 arg3)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue && existingDelegate != null)
            {
                //existingDelegate.DynamicInvoke(arg0, arg1, arg2, arg3);
                Delegate[] delegates = existingDelegate.GetInvocationList();
                foreach (Delegate tempDelegate in delegates)
                {
                    try
                    {
                        tempDelegate.DynamicInvoke(arg0, arg1, arg2, arg3);
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        Delegate.Remove(existingDelegate, tempDelegate);
                    }
                }
            }
        }

        public static void RemoveEvent(string eventName, Delegate remaveAction)
        {
            Delegate existingDelegate;
            bool hasValue = delegateDic.TryGetValue(eventName, out existingDelegate);
            if (hasValue)
            {
                Delegate.Remove(existingDelegate, remaveAction);
            }
        }
    }
}