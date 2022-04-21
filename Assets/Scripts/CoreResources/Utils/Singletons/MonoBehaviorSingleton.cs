using System;
using System.Collections.Generic;
using CoreResources.Handlers.EventHandler;
using CoreResources.Utils.Disposables;
using UnityEngine;

namespace CoreResources.Utils.Singletons
{
    public abstract class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviorSingleton<T>
    {
        private static T _instance;
        protected List<IDisposable> _disposables;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                
                return _instance;
            }
        }

        protected virtual void InitSingleton()
        {
            _disposables = new List<IDisposable>();
            
            if (Instance != null && GetInstanceID() != Instance.GetInstanceID())
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        protected virtual void CleanSingleton()
        {
            if (_disposables != null)
            {
                _disposables.ClearDisposables();
            }
        }

        protected virtual void Awake()
        {
            InitSingleton();
        }

        protected virtual void OnDestroy()
        {
            CleanSingleton();
        }

        protected virtual void OnReset(REvent evt)
        {
            
        }
    }
}