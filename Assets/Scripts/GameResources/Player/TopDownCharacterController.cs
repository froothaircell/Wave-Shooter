using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class TopDownCharacterController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float dodgeDistance = 10f;
        public float dodgeSpeed = 10f;
        public Rigidbody rb;

        private float _lookSmoothing = 0.8f;
        private Camera cam;
        private Vector3 _movement;
        private Vector3 _mousePos;
        private bool _willDodge;
        private Func<bool> _dodgeConsumeAction;
        private Coroutine _dodgeCoroutine;

        private RaycastHit _movementCollision;

        private void Start()
        {
            cam = Camera.main;
            _dodgeConsumeAction = GetComponent<PlayerStats>().ConsumeDodge;
            _willDodge = false;
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement = _movement.normalized;

            if (Physics.Raycast(rb.position, rb.position + _movement, out _movementCollision, _movement.magnitude))
            {
                _movement = _movementCollision.point - rb.position;
                _movement.z = 0;
            }

            if (Input.GetButtonDown("Jump") && _movement.magnitude > 0.9f)
            {
                if (!_willDodge && _dodgeConsumeAction())
                {
                    _willDodge = true;
                    _dodgeCoroutine = StartCoroutine(DodgeSequence(_movement));
                }
            }

            var currMousePos = Input.mousePosition;
            currMousePos.z = 50; // Change this later it's wonky
            _mousePos = cam.ScreenToWorldPoint(currMousePos);
            
            Vector2 lookDir = _mousePos - rb.position;
            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation, 
                Quaternion.LookRotation(lookDir, -Vector3.forward),
                _lookSmoothing));
        }

        private void FixedUpdate()
        {
            if (!_willDodge)
            {
                rb.MovePosition(rb.position + _movement * moveSpeed * Time.deltaTime);
            }
        }

        private IEnumerator DodgeSequence(Vector3 movement)
        {
            var firstPos = rb.position;
            movement *= dodgeSpeed;
            movement.z = 0;
            while (Vector3.Distance(transform.position, firstPos) < dodgeDistance)
            {
                rb.MovePosition(rb.position + movement * Time.smoothDeltaTime);
                yield return new WaitForFixedUpdate();
            }
            _willDodge = false;
        }
        
        
    }
}
