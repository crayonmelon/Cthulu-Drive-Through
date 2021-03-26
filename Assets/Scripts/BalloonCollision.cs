using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonCollision : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;
    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        sphereCollider = gameObject.GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine("balloonRespawn", 1);
        }
    }
    private IEnumerator balloonRespawn(float waitTime)
    {
        meshRenderer.enabled = false;
        sphereCollider.enabled = false;
        yield return new WaitForSeconds(waitTime);
        meshRenderer.enabled = true;
        sphereCollider.enabled = true;
    }
}
