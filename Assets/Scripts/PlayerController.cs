using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float jump = 30;
    [SerializeField] private float speen = 15;
    [SerializeField] private float speed = 5;
    [SerializeField] private float slowSpeed = 1;
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float friction = .95f;

    [Header("Necc")]
    [SerializeField] private float groundDistance = .5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject playerBody;

    [Header("Camera stuff")]
    [SerializeField] private Transform cam;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Text")]
    [SerializeField] private TextMeshPro text;
    
    private bool isJumpPressed = false;
    private bool isSlowedDown = false;
    private Vector3 direction;
    private Rigidbody rb;
    private float turnSmoothVelocity;
    private float angle;
    private float SpeedLimit;

    //Trick Values
    private float Yangle;
    private float Xangle;

    private bool YFlagOne = false;
    private bool YFlagTwo = false;

    private bool XFlagOne = false;
    private bool XFlagTwo = false;

    private int trickFlipsAmount = 0;
    private int trickSpeensAmount = 0;
    private int pedestrianHit = 0;

    private string trickSpeenText;
    private string trickFlipsText;
    private string pedestrianHitText;

    float countTimePedHit;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        SpeedLimit = maxSpeed;
        GameEvents.current.onPedestrainHit += OnPedestrainHit;
    }

    private void Update()
    {
        pedestrainCounterReset();

        isJumpPressed = Input.GetButtonDown("Jump");
        isSlowedDown = Input.GetButton("slowDown");

        if (IsGrounded() && isJumpPressed)
        {
            rb.AddForce(Vector3.up * jump, ForceMode.VelocityChange);
        }

        if(IsGrounded() && isSlowedDown)
        {
            if (SpeedLimit > slowSpeed)
            {
                SpeedLimit-= Time.deltaTime*20;
            }

        }
        else
        {
            SpeedLimit = maxSpeed;
        }
    }

    void FixedUpdate()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(playerBody.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        if (IsGrounded())
        {
            ResetTricks(); 
            GroundHug();

            rb.constraints = RigidbodyConstraints.FreezeRotation;

            playerBody.transform.localRotation = Quaternion.Euler(
                playerBody.transform.localRotation.x,
                angle,
                playerBody.transform.rotation.z);

            if (direction.magnitude >= 0.1f)
            {
                rb.AddForce(playerBody.transform.forward * speed, ForceMode.VelocityChange);
            }
        }
        else
        {
            CalculateTricks();

            rb.constraints = RigidbodyConstraints.None;
            if (direction.magnitude >= 0.1f)
            {
                rb.AddRelativeTorque(playerBody.transform.right * direction.z * speen, ForceMode.Acceleration);
                rb.AddRelativeTorque(playerBody.transform.up * direction.x * speen, ForceMode.Acceleration);
            }
        }
        //setting the max speed
        if (rb.velocity.magnitude > SpeedLimit) 
            rb.velocity = rb.velocity.normalized * SpeedLimit;

        UpdateTrickText();
    }

    /// <summary>
    /// to calculate when the player does a trick;
    /// Flip: a full rotation on the Y axis
    /// Speen: a full rotation on the X axis
    /// </summary>
    private void CalculateTricks()
    {
        Yangle = transform.rotation.eulerAngles.y;
        Xangle = playerBody.transform.rotation.eulerAngles.x;

        if (Xangle > 40f && Xangle < 120f)
        {
            Debug.Log("spin 90");
            XFlagOne = true;
        }

        if (Xangle > 160f && Xangle < 300f)
        {
            Debug.Log("spin 180");
            XFlagTwo = true;
        }

        if (Xangle > 350 & XFlagOne & XFlagTwo)
        {
            trickFlipsAmount++;
            Debug.Log("Spin " + trickFlipsAmount);
            XFlagOne = false;
            XFlagTwo = false;
        }

        if (Yangle > 40f && Yangle < 120f)
        {
            Debug.Log("spin 90");
            YFlagOne = true;
        }

        if (Yangle > 160f && Yangle < 300f)
        {
            Debug.Log("spin 180");
            YFlagTwo = true;
        }

        if (Yangle > 350 & YFlagOne & YFlagTwo)
        {
            trickSpeensAmount++;
            Debug.Log("Spin " + trickSpeensAmount);
            YFlagOne = false;
            YFlagTwo = false;
        }
    }

    /// <summary>
    /// Reset all values for calculating the tricks
    /// called when the player hits the ground
    /// </summary>
    private void ResetTricks()
    {
        if(trickSpeensAmount != 0f || trickFlipsAmount != 0f)
        {
            GameEvents.current.ScoreChanged(trickSpeensAmount + trickFlipsAmount);

        }

        XFlagTwo = false;
        XFlagOne = false;
        trickFlipsAmount = 0;

        YFlagTwo = false;
        YFlagOne = false;
        trickSpeensAmount = 0;

        trickSpeenText = "";
        trickFlipsText = "";

    }

    /// <summary>
    /// Updates the trick text in the bottom left
    /// </summary>
    private void UpdateTrickText()
    {
        if(trickFlipsAmount > 0)
            trickFlipsText = " Flips " + trickFlipsAmount + "<br>";

        if (pedestrianHit > 0)
        {
            string letterO = "";

            for (int i = 0; i < pedestrianHit; i++)
            {
                letterO = letterO + "o";
            }
            pedestrianHitText = letterO + "ops" + "<br>";
        }

        if (trickSpeensAmount > 0)
        {
            string letterE = "";

            for(int i = 0; i < trickSpeensAmount; i++)
            {
                letterE = letterE + "e";
            }
            trickSpeenText = " Spe"+ letterE + "n!" + "<br>";

        }

        text.SetText(trickFlipsText + trickSpeenText + pedestrianHitText);
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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * groundDistance, Color.red);
        Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.red);
        return Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask) || Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), groundDistance, groundMask);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rb.AddForce(transform.up * 18, ForceMode.VelocityChange);
            //rb.AddForce(transform.forward * 18, ForceMode.VelocityChange);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balloon"))
        {
            rb.AddForce(Vector3.up * 20, ForceMode.VelocityChange);
        }
    }
    private void OnPedestrainHit(int hit)
    {
        countTimePedHit = 2.0f;
        pedestrianHit++;
    }

    private void pedestrainCounterReset()
    {
        if (countTimePedHit > 0)
        {
            countTimePedHit -= Time.deltaTime;
        } else
        {
            pedestrianHit = 0;
            pedestrianHitText = "";
        }
    }
}