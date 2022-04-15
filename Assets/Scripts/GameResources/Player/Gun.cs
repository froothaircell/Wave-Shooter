using System;
using System.Collections;
using System.Collections.Generic;
using GameResources.Player;
using UnityEngine;

// Change to non monobehavior after you have a pool manager
public class Gun : MonoBehaviour, IGun
{
    public float bulletSpeed = 10f;
    public GameObject bullet;
    private Transform _firePoint;
    
    public void OnInit()
    {
        _firePoint = transform.GetChild(0);
        GetComponentInParent<GunController>().OnFire += FireBullet;
    }

    public void OnDeInit()
    {
        GetComponentInParent<GunController>().OnFire -= FireBullet;
    }

    private void FireBullet()
    {
        Debug.Log($"Fired bullet with the current direction: {_firePoint.up * bulletSpeed}");
        var projectile = Instantiate(bullet, _firePoint.position, _firePoint.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(_firePoint.up * bulletSpeed, ForceMode.VelocityChange);
    }

}
