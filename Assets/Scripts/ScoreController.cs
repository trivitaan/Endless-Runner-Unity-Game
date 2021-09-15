using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;

    private int currentScore = 0;

    public int GetCurrentScore()
    {
        return currentScore;
    }
    public void IncreaseCurrentScore(int increment)
    {
        currentScore += increment;
    }

    public void FinishScoring()
    {
        //set high score
        if(currentScore > ScoreData.highScore)
        {
            ScoreData.highScore = currentScore;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //reset
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //read input
        
    }
}
