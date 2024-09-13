using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static void CheckNewRecord(int currentScore)
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            PlayerPrefs.SetString("BestScoreDate", DateTime.Now.ToString("yyyy-MM-dd"));
            PlayerPrefs.Save();  
        }
    }

    public static (int, string) GetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        string bestScoreDate = PlayerPrefs.GetString("BestScoreDate", "N/A");
        return (bestScore, bestScoreDate);
    }
}
