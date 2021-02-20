using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollison : MonoBehaviour
{
    [SerializeField] private float forceUp = 5f;
    private Rigidbody rb;
    private LookAtCamera lookAtCamera;
    private Transform childSprite;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        childSprite = this.gameObject.transform.GetChild(0);
        lookAtCamera = childSprite.GetComponent<LookAtCamera>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision" + collision.transform.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints.None;

            rb.AddForce(transform.up * forceUp, ForceMode.VelocityChange);
            rb.AddTorque(transform.up * forceUp, ForceMode.VelocityChange);
            
            lookAtCamera.enabled = false;
            childSprite.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
