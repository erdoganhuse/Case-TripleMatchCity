using System;
using System.Collections;
using Library.Utility;
using UnityEngine;

namespace Library.CoroutineSystem
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void BeginCoroutine(string method)
        {
            Instance.StartCoroutine(method);
        }
        
        public static Coroutine BeginCoroutine(IEnumerator method)
        {
            return Instance.StartCoroutine(method);
        }
        
        public static void EndCoroutine(Coroutine method)
        {
            if (method != null)
            {
                Instance.StopCoroutine(method);
            }

            method = null;
        }

        public static void EndCoroutine(string method)
        {
            Instance.StopCoroutine(method);
        }
        
        public static Coroutine DoAfterWaitUntil(Func<bool> condition, Action actionToInvoke)
        {
            return Instance.StartCoroutine(WaitUntil(condition, actionToInvoke));
        }
        
        public static Coroutine DoAfterFixedUpdate(Action actionToInvoke)
        {
            return  Instance.StartCoroutine(Wait(Time.fixedDeltaTime, actionToInvoke));
        }

        public static Coroutine DoAfterGivenTime(float waitTime, Action actionToInvoke)
        {
            return Instance.StartCoroutine(Wait(waitTime, actionToInvoke));
        }
        
        public static Coroutine DoAfterGivenUnscaledTime(float waitTime, Action actionToInvoke)
        {
            return Instance.StartCoroutine(WaitUnscaled(waitTime, actionToInvoke));
        }
        
        private static IEnumerator Wait(float time, Action actionToInvoke)
        {
            yield return new WaitForSeconds(time);
            
            actionToInvoke.Invoke();
        }        
        
        private static IEnumerator WaitUnscaled(float time, Action actionToInvoke)
        {
            yield return new WaitForSecondsRealtime(time);

            actionToInvoke.Invoke();
        }

        private static IEnumerator WaitUntil(Func<bool> condition, Action actionToInvoke)
        {
            yield return new WaitUntil(condition);
            
            actionToInvoke.Invoke();
        }
    }
}