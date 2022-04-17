using System;
using UnityEngine;

namespace GameResources.Bullet
{
    public abstract class RBullet : MonoBehaviour, IBullet
    {
        public int Damage { get; private set; }
        public float BulletLifetime = 1f; // Despawn the bullet after this amount of time

        public int SpawnInd { get; private set; }
        protected Rigidbody rb;
        protected float BulletSpeed;
        protected float LocalTime = 0f; // Every bullet should have a unique timer;
        protected float RealTime;

        public virtual void OnInit()
        {
            LocalTime = 0f;
            RealTime = Time.realtimeSinceStartup;
            rb = GetComponent<Rigidbody>();
        }

        public virtual void OnUpdate()
        {
            var currRT = Time.realtimeSinceStartup;
            var currTimeDif = currRT - RealTime;
            RealTime = currRT;
            LocalTime += currTimeDif;
            if (LocalTime >= BulletLifetime)
            {
                ReturnToPool();
            }
            rb.MovePosition(rb.position + transform.up * BulletSpeed * currTimeDif);
        }

        public virtual void OnDeInit()
        {
            SpawnInd = -1;
        }

        public virtual void SetSpawnedBulletSpecs(int dmg, float speed, int spawnInd)
        {
            Damage = dmg;
            BulletSpeed = speed;
            SpawnInd = spawnInd;
        }

        private void ReturnToPool()
        {
            Debug.Log("Returning bullet to pool");
            AppHandler.BulletManager.ReturnToPool(gameObject);
        }
    }
}