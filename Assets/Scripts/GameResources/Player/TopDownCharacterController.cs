using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Player
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float dodgeDistance = 10f;
        public float dodgeSpeed = 10f;
        public Rigidbody rb;

        private Camera cam;
        private Vector3 _movement;
        private Vector3 _mousePos;
        private bool _willDodge;
        private Coroutine dodgeCoroutine;

        private void Start()
        {
            cam = Camera.main;
            _willDodge = false;
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement = _movement.normalized;

            if (Input.GetButtonDown("Jump") && _movement.magnitude > 0.9f)
            {
                if (!_willDodge)
                {
                    _willDodge = true;
                    dodgeCoroutine = StartCoroutine(DodgeSequence(_movement));
                }
            }

            var currMousePos = Input.mousePosition;
            currMousePos.z = 30 - 4; // Change this later it's wonky
            _mousePos = cam.ScreenToWorldPoint(currMousePos);
            
            Vector2 lookDir = _mousePos - rb.position;
            rb.MoveRotation(Quaternion.LookRotation(lookDir, -Vector3.forward));
            // transform.rotation = Quaternion.LookRotation(lookDir, -Vector3.forward);
        }

        private void FixedUpdate()
        {
            if (!_willDodge)
            {
                rb.MovePosition(rb.position +_movement * moveSpeed * Time.deltaTime);
                // transform.position += _movement * (moveSpeed * Time.deltaTime);
                // rb.AddForce(_movement * moveSpeed, ForceMode.VelocityChange);
            }
        }

        private IEnumerator DodgeSequence(Vector3 movement)
        {
            var firstPos = rb.position;
            movement *= dodgeSpeed;
            movement.z = 0;
            while (Vector3.Distance(transform.position, firstPos) < dodgeDistance)
            {
                rb.MovePosition(rb.position + movement * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            _willDodge = false;
        }
        
        
    }
}
