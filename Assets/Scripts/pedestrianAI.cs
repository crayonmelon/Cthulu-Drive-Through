using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pedestrianAI : MonoBehaviour
{
    [SerializeField] private float forceUp = 5f;
    
    private Rigidbody rb;
    private LookAtCamera lookAtCamera;
    private Transform childSprite;
    private PedestrianDestination pedestrianDestination;
    private NavMeshAgent agent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        childSprite = this.gameObject.transform.GetChild(0);
        lookAtCamera = childSprite.GetComponent<LookAtCamera>();
        pedestrianDestination = GetComponent<PedestrianDestination>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision" + collision.transform.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            lookAtCamera.enabled = false;
            pedestrianDestination.enabled = false;
            agent.enabled = false;
            childSprite.transform.eulerAngles = new Vector3(0, 0, 0);

            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;

            rb.AddForce(transform.up * forceUp, ForceMode.VelocityChange);
            rb.AddTorque(transform.up * forceUp, ForceMode.VelocityChange);

        }
    }
}
