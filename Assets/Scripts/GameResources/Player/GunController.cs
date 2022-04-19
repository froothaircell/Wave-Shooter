using System;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Player
{
    public class GunController : MonoBehaviour, IPlayerComponent
    {
        public Action OnFire;
        public Action OnFireSpec;

        public void OnInit()
        {
            
        }

        public void OnUpdate()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                OnFire.Invoke();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                OnFireSpec.Invoke();
            }
        }

        public void OnDeInit()
        {
            OnFire = null;
            OnFireSpec = null;
        }
    }
}
