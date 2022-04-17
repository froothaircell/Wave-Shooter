using System;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Player
{
    public class GunController : MonoBehaviour
    {
        public Action OnFire;
        public Action OnFireSpec;

        private void Start()
        {
            GetComponentInChildren<IGun>().OnInit();
        }
        
        private void Update()
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

        private void OnDisable()
        {
            OnFire = null;
            OnFireSpec = null;
        }
    }
}
