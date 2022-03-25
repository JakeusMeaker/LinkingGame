using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int currentScore = 0;

    public static void AddToScore(int score)
    {
        currentScore += score;
    }

    public static void ResetScore()
    {
        currentScore = 0;
    }
}
