using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stayOnParent : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 direction;
    [SerializeField] private float speed = 2f;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position = transform.parent.position;
        transform.Rotate(0, 360 * Time.deltaTime * speed, 0);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("ohno");
        /*
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 18, ForceMode.VelocityChange);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * 18, ForceMode.VelocityChange);
            collision.gameObject.GetComponent<Rigidbody>().AddTorque(transform.right * 18, ForceMode.VelocityChange);
            Debug.Log("hello");
        }
        */
    }


}






