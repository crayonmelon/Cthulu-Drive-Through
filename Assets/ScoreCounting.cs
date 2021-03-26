using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounting : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        GameEvents.current.onScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(float score)
    {
        Debug.Log("hold");
        scoreText.SetText("Score " + score);
    }
}
