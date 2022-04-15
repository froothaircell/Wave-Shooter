using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dodgeSpeed = 10f;
    public Rigidbody rb;

    private Camera cam;
    private Vector3 _movement;
    private Vector3 _mousePos;
    private bool _willDodge;

    private void Start()
    {
        cam = Camera.main;
        _willDodge = false;
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            _willDodge = true;
        }

        var currMousePos = Input.mousePosition;
        currMousePos.z = 30 - 4;
        _mousePos = cam.ScreenToWorldPoint(currMousePos);
    }

    private void FixedUpdate()
    {
        if (_willDodge)
        {
            _willDodge = false;
            _movement *= dodgeSpeed;
            _movement.z = 0;
            transform.Translate(_movement, Space.World);
        }
        
        // rb.MovePosition(rb.position +_movement * moveSpeed * Time.deltaTime);
        transform.position = transform.position + _movement * moveSpeed * Time.deltaTime;
        
        Vector2 lookDir = _mousePos - rb.position;
        
        transform.rotation = Quaternion.LookRotation(lookDir, -Vector3.forward);
    }
}
