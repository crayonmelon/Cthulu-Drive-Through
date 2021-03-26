using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianDestination : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject WaypointParent;
    private Vector3 waypoint;
    private int randomPoint;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        WaypointParent = GameObject.FindGameObjectWithTag("WanderWaypointParent");
        newPoint();
    }

    private void Update()
    {
        if (Vector3.Distance(waypoint, transform.position) < 5f)
             newPoint();
    }

    private void newPoint()
    {
        randomPoint = Random.Range(0, WaypointParent.transform.childCount);
        waypoint = WaypointParent.transform.GetChild(randomPoint).position;
        agent.SetDestination(waypoint);
    }
}
