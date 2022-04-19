using System;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Player
{
    public class PlayerController : MonoBehaviour
    {
        private IPlayerComponent[] _components;
        
        private void Awake()
        {
            _components = GetComponentsInChildren<IPlayerComponent>();
            foreach (var component in _components)
            {
                component.OnInit();
            }
        }

        private void Update()
        {
            foreach (var component in _components)
            {
                component.OnUpdate();
            }
        }

        private void OnDisable()
        {
            foreach (var component in _components)
            {
                component.OnDeInit();
            }
        }
    }
}