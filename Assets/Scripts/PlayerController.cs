using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float jump = 30;
    [SerializeField] private float speed = 15;
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float groundDistance = .5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject playerBody;

    [Header("Camera stuff")]
    [SerializeField] private Transform cam;
    [SerializeField] private float turnSmoothTime = 0.1f;


    float turnSmoothVelocity;
    float angle;
    private bool isJumpPressed = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isJumpPressed = Input.GetButtonDown("Jump");
        if (IsGrounded() && isJumpPressed)
        {
            Debug.Log("yo");
            rb.AddForce(Vector3.up * jump, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        GroundHug();

        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (direction.magnitude >= 0.1f & IsGrounded())
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(playerBody.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
           
            rb.AddForce(playerBody.transform.forward * speed, ForceMode.VelocityChange);

            playerBody.transform.localRotation = Quaternion.Euler(
                playerBody.transform.localRotation.x,
                angle,
                playerBody.transform.rotation.z);
        }

        
        //setting the max speed
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;

    }
   
   /// <summary>
   /// used to keep the player parellel to the ground
   /// </summary>
    private void GroundHug()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundMask);
        transform.up -= (transform.up - hit.normal) * 0.1f;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }
}
