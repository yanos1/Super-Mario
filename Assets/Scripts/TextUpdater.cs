using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
public class TextUpdater : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI worldText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;


   
    public void UpdateScore(int newScore)
    {
        scoreText.text = $"SCORE\r\n    {newScore}";
    }
    public void UpdateCoin(int newCoins)
    {
        coinText.text = $"COINS\r\n     {newCoins}";
    }
    public void UpdateWorld(string newWorld)
    {
        worldText.text = $"WORLD\r\n     1-{newWorld}";
    }
    public void UpdateTime(int newTime)
    {
        timeText.text = $"TIME\r\n  {newTime}";
    }
    public void UpdateLives(int newLives)
    {
        livesText.text = $"LIVES\r\n    {newLives}";
    }
}
