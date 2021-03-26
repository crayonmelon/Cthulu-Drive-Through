using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public float totalScore;
    public int totalpedestrianHits;

    //EVENTS
    public event Action<float> onScoreChanged;
    public event Action<int> onPedestrainHit;
     
    public void ScoreChanged(float score)
    {
        totalScore += score;
        onScoreChanged?.Invoke(totalScore);
    }

    public void PedestrainHit(int hit)
    {
        Debug.Log("pedestrianCount: " + totalpedestrianHits);
        totalpedestrianHits += hit;
        onPedestrainHit?.Invoke(totalpedestrianHits);
    }
}
