using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;
    
    [Header("Score Highlight")]
    public int scoreHighlightRange;
    public CharacterSoundController sound;

    private int lastScoreHighlight = 0;

    private int currentScore = 0;

    public int GetCurrentScore()
    {
        return currentScore;
    }
    public void IncreaseCurrentScore(int increment)
    {
        currentScore += increment;
        if(currentScore - lastScoreHighlight > scoreHighlightRange)
        {
            sound.PlayScoreHighlight();
            lastScoreHighlight += scoreHighlightRange;
        }
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
        lastScoreHighlight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //read input
        
    }
}
