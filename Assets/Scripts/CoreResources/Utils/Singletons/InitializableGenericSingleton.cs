﻿using System;
using System.Collections.Generic;
using CoreResources.Handlers.EventHandler;
using CoreResources.Utils.Disposables;
using GameResources;
using GameResources.Events;

// using GameResources;
// using GameResources.Events;

namespace CoreResources.Utils.Singletons
{
    public abstract class GenericSingleton
    {
        protected abstract void InitSingleton();
        protected abstract void CleanSingleton();
    }
    
    public abstract class InitializableGenericSingleton<T> : GenericSingleton where T : InitializableGenericSingleton<T>
    {
        protected List<IDisposable> _disposables;
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"Accessing {typeof(T).Name} before {typeof(T).Name}.");
                }
                
                return _instance;
            }
        }

        public static bool IsInstantiated => _instance != null;

        public static TCustom SetInstanceType<TCustom>() where TCustom : T, new()
        {
            TCustom newInstance;

            if (_instance == null)
            {
                newInstance = new TCustom();
                _instance = newInstance;
                _instance.InitSingleton();
            }
            else
            {
                newInstance = _instance as TCustom;

                if (newInstance == null)
                {
                    _instance.CleanSingleton();
                    newInstance = new TCustom();
                    _instance = newInstance;
                    _instance.InitSingleton();
                }
            }

            return newInstance;
        }

        protected override void InitSingleton()
        {
            _disposables = new List<IDisposable>();
            AppHandler.EventManager.Subscribe<REvent_GameQuit>(OnCleanSingleton, _disposables);
        }

        protected override void CleanSingleton()
        {
            if (_disposables != null)
            {
                _disposables.ClearDisposables();
            }
        }

        private void OnCleanSingleton(REvent evt)
        {
            CleanSingleton();
        }
    }
}