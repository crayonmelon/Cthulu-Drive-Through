using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stack : MonoBehaviour
{

    public int stackAmount = 1;
    [SerializeField] private GameObject stackCarPrefab;
    [SerializeField] private float carHeight = 1;
    private int stacked = 1;

    void Start()
    {
        addStackAmount();
    }

   private void addToStack(int stackLevel)
    {
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + carHeight * (stackLevel), transform.position.z);
        GameObject child = Instantiate(stackCarPrefab, spawnLocation, transform.rotation);
        child.transform.parent = transform;
        stacked++;
    }

    public void addStackAmount()
    {
        for (int i = stacked; i < stackAmount; i++)
        {
            addToStack(i);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.GetComponent<stack>() != null)
            {
                stack collidedStackScript = collision.gameObject.GetComponent<stack>();

                if (collidedStackScript.stackAmount >= stackAmount)
                {
                    collidedStackScript.stackAmount += stacked;
                    collidedStackScript.addStackAmount();
                    Destroy(gameObject);
                }
            }
        }
    }
}