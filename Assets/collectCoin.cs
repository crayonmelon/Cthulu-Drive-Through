using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectCoin : MonoBehaviour
{
    [SerializeField] private float scoreWorth = 1f;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEvents.current.ScoreChanged(scoreWorth);
            Destroy(gameObject);
        }
    }
}
