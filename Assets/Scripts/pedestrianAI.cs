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
        if ((collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Enemy")) && agent.enabled == true)
        {
            if (collision.gameObject.CompareTag("Player"))
                GameEvents.current.PedestrainHit(1);
            //disable components so pedestrains can ragdoll
            lookAtCamera.enabled = false;
            pedestrianDestination.enabled = false;
            agent.enabled = false;

            //reset position of sprite so it lines up with collider
            childSprite.transform.localEulerAngles = new Vector3(0, 0, 0);

            //remove constrains to allow pedestrain to ragdoll
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;

            //launch pedestrain into the sky 
            rb.AddForce(transform.up * forceUp, ForceMode.VelocityChange);
            rb.AddTorque(transform.up * forceUp, ForceMode.VelocityChange);

        }
        
    }
}
