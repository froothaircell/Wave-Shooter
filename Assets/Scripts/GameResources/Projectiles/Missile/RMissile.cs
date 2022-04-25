using System;
using System.Collections;
using BulletFury;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Projectiles.Missile
{
    public enum MissileState
    {
        Loaded = 0,
        Firing = 1,
        Pooled = 2,
    }
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BulletManager))]
    public abstract class RMissile : MonoBehaviour, IPooledProjectile
    {
        public int Damage { get; protected set; }
        public virtual MissileState CurrentMissileState
        {
            get
            {
                var joint = GetComponent<FixedJoint>();
                if (joint == null)
                {
                    if (MissileCoroutine == null)
                        return MissileState.Pooled;
                    else
                        return MissileState.Firing;
                }
                else
                {
                    return MissileState.Loaded;
                }
            }
        }
        public float MissileLifeTime = 5f;

        protected Rigidbody rb;
        protected FixedJoint fj;
        protected BulletManager ExplosionManager;
        protected Transform Target;
        protected float MissileSpeed;
        protected float TrackingFactor = 0.02f;
        protected Coroutine MissileCoroutine;

        public virtual void OnSpawn()
        {
            rb = GetComponent<Rigidbody>();
            fj = GetComponent<FixedJoint>();
            Target = AppHandler.CharacterManager.PlayerShip.transform;
            ExplosionManager = GetComponent<BulletManager>();
        }

        public virtual void OnSpawnedUpdate()
        {
            
        }

        public virtual void OnDespawn()
        {
            if (MissileCoroutine != null)
            {
                StopCoroutine(MissileCoroutine);
                MissileCoroutine = null;
            }
        }

        public virtual void LoadMissile(Transform missileSlot)
        {
            transform.position = missileSlot.position;
            transform.rotation = missileSlot.rotation;
            fj = gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = missileSlot.GetComponentInParent<Rigidbody>();
            fj.connectedArticulationBody = missileSlot.GetComponent<ArticulationBody>();
            fj.connectedAnchor = missileSlot.position;
            fj.anchor = missileSlot.position;
            fj.axis = missileSlot.rotation.eulerAngles;
        }

        public virtual void ReturnToPool()
        {
            AppHandler.BulletManager.ReturnMissileToPool(gameObject);
        }

        public virtual void FireMissile()
        {
            if (CurrentMissileState == MissileState.Loaded)
            {
                Destroy(fj);
                fj = null;
                MissileCoroutine = StartCoroutine(GuidedMissileCoroutine());
            }
        }
        
        protected virtual void Explode()
        {
            ExplosionManager.Spawn(transform.position, transform.forward);
        }

        protected abstract IEnumerator GuidedMissileCoroutine();

        protected virtual void OnCollisionEnter(Collision collision)
        {
            Explode();
            ReturnToPool();
        }
    }
}