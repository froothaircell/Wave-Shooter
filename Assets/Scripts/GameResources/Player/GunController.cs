using System;
using System.Collections;
using System.Collections.Generic;
using GameResources.Player;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Action OnFire;

    private void Start()
    {
        GetComponentInChildren<IGun>().OnInit();
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1 detected");
            OnFire.Invoke();
        }
    }

    private void OnDisable()
    {
        GetComponentInChildren<IGun>().OnDeInit();
    }
}
