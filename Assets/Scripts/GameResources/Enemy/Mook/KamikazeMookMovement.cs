using GameResources.Character;
using UnityEngine;

namespace GameResources.Enemy.Mook
{
    [RequireComponent(typeof(Rigidbody))]
    public class KamikazeMookMovement : MonoBehaviour, ICharacterComponent
    {
        public float Speed = 1f;
        public float SmoothingFactor = 0.5f;
        
        private Transform _target;
        private Rigidbody rb;
        
        public void OnInit()
        {
            _target = AppHandler.CharacterManager.PlayerShip.transform;
            rb = GetComponent<Rigidbody>();
        }

        public void OnUpdate()
        {
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation, 
                Quaternion.LookRotation(_target.position - rb.position, -Vector3.forward),
                SmoothingFactor));
            rb.MovePosition(rb.position + transform.forward * Speed * Time.smoothDeltaTime);
        }

        public void OnDeInit()
        {
            _target = null;
        }
    }
}